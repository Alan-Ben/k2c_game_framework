package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.PlayerObj.Player_MarsEnergyInfo;
import Common.ServerObj.ServerObj_SpecialItem_MarsEnergy;
import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import NPCommon.DB.BM.BM;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent._ASpecialItemDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USDB.Bo.PlayerSpecialItemBO;
import USLOGDB.Bo.LogCurrencyMarsEnergyBO;

import java.nio.ByteBuffer;


public class SpecialItemDealer_MarsEnergy extends _ASpecialItemDealer implements _IHandlerHolder
{
	//数据ID
    private long _m_lDbId;
    //上次结算时间（毫秒）
    private long _m_lLastSettleTimeMs;
    //当前数量
    private long _m_lCount;
    //消耗速度（单位：分钟）
    private long _m_lCostSpeed;

    private long _m_lTotalGainCount;//总获得数量
    private long _m_lTotalConsumeCount;//总消耗数量

    //计算消耗速度
    private LazyTaskDealer _m_ldCalCostSpeedLazyDealer;
    //结算能量值
    private LazyTaskDealer _m_ldSettleLazyDealer;
    //保存数据到数据库的处理，减少数据库操作的次数
    private LazyTaskDealer _m_ldSaveUpdateLazyDealer;

    public SpecialItemDealer_MarsEnergy(SpecialItemComponent _comp)
    {
        super(_comp);

        _m_lDbId = 0;
        _m_lLastSettleTimeMs = CommonFunc.getNowTimeMS();
        _m_lCount = 0;
        _m_lCostSpeed = 0;
        
        _m_lTotalGainCount = 0;
        _m_lTotalConsumeCount = 0;

        //延迟处理器
        _m_ldCalCostSpeedLazyDealer = new LazyTaskDealer(() -> _doCalCostSpeed(), 100);
        _m_ldSettleLazyDealer = new LazyTaskDealer(() -> _doSettle(false), 100);
        _m_ldSaveUpdateLazyDealer = new LazyTaskDealer(() -> _doSaveUpdate(), 500);
    }

    public long getTotalGainCount() {return _m_lTotalGainCount;}
    public long getTotalConsumeCount() {return _m_lTotalConsumeCount;}
    
    public void doLazyCalCostSpeed() {_m_ldCalCostSpeedLazyDealer.setNeedDeal();}
    public void doLazySettle() {_m_ldSettleLazyDealer.setNeedDeal();}

    @Override
    public ESpecialItemType getSpecialItemType() {return ESpecialItemType.MARS_ENERGY;}

    @Override
    public void initBo(PlayerSpecialItemBO _bo)
    {
    	_m_lDbId = _bo.getId();

        if (_bo.getData() != null)
        {
            ServerObj_SpecialItem_MarsEnergy proto = new ServerObj_SpecialItem_MarsEnergy();
            proto.readPackage(ByteBuffer.wrap(_bo.getData()));

            _m_lLastSettleTimeMs = proto.getLastSettleTimeMs();
            _m_lCount = proto.getCount();
            _m_lCostSpeed = proto.getCostSpeed();

            _m_lTotalGainCount = proto.getTotalGainCount();
            _m_lTotalConsumeCount = proto.getTotalConsumeCount();
        }
    }

    @Override
    public void onInited()
    {
    	//执行一次结算，对离线前的产出进行更新
    	_doSettle(true);
    }

    @Override
    public void dispose()
    {
    }
    
    /**
     * 保存数据
     */
    private void _saveData()
    {
    	_lock();
    	
    	try
    	{
    		if (0 == _m_lDbId)
            {
                BM bmObj = getComp().getUSServer().getBM();
                //构造保存数据
                ServerObj_SpecialItem_MarsEnergy proto = new ServerObj_SpecialItem_MarsEnergy();
                proto.setLastSettleTimeMs(_m_lLastSettleTimeMs);
                proto.setCostSpeed(_m_lCostSpeed);
                proto.setCount(_m_lCount);
                proto.setTotalGainCount(_m_lTotalGainCount);
                proto.setTotalConsumeCount(_m_lTotalConsumeCount);

                PlayerSpecialItemBO bo = new PlayerSpecialItemBO();
                bo.setCid(bmObj, getComp().getUserData().getCid());
                bo.setType(bmObj, getSpecialItemType().ordinal());
                bo.setData(bmObj, CommonFunc.ByteBfferToBytes(proto.makePackage()));
                bo.insert(bmObj);

                _m_lDbId = bo.getId();
            } 
            else
            {
            	//延迟处理数据更新
            	_m_ldSaveUpdateLazyDealer.setNeedDeal();
            }
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 更新数据
     */
    private void _doSaveUpdate()
    {
    	_lock();
    	
    	try
    	{
    		BM bmObj = getComp().getUSServer().getBM();
            //构造保存数据
    		ServerObj_SpecialItem_MarsEnergy proto = new ServerObj_SpecialItem_MarsEnergy();
            proto.setLastSettleTimeMs(_m_lLastSettleTimeMs);
            proto.setCostSpeed(_m_lCostSpeed);
            proto.setCount(_m_lCount);
            proto.setTotalGainCount(_m_lTotalGainCount);
            proto.setTotalConsumeCount(_m_lTotalConsumeCount);

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("data", proto.makePackage());

            bmObj.getBM(PlayerSpecialItemBO.class).update("id", _m_lDbId, updateValue);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 获取全部消耗速度
     * 
     * 主基地的单位消耗数值 + 其他所有建筑下属的部件产生的消耗
     * 
     * @return
     */
    public long getTotalCostSpeed()
    {
    	return getUserData().getMarsBuildingComponent().getAllBuildingValue().getEnergyConsumePerMin();
    }
    
    /**
     * 触发消耗速度改变
     */
    private void _doCalCostSpeed()
    {
    	_lock();
    	
    	try
    	{
    		long newCostSpeed = getTotalCostSpeed();
    		
    		//速度一致，不予处理
    		if(newCostSpeed == _m_lCostSpeed)
    			return;
    		
    		//进行一次结算
    		_doSettle(false);
    		
    		//更新数据
    		_m_lCostSpeed = newCostSpeed;
    		_saveData();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_055_OnMarsEnergyChg(this));

			//日志数据
			_log(0, NPPlayerContext.createNew(ENPGameEvent.MARS_ENERGY_SPEED_CHG));
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 执行一次消耗
     *
     * 结算流程
        1. 首次进入结算，设置结算开始时间点，直接返回
        2. 距离上次结算时长不满一分钟，直接返回
        3. 计算结算消耗数量，更新上次结算时长（距离当前时间最近的分钟整数倍）
     *
     * @param _isInit
     */
    private void _doSettle(boolean _isInit)
    {
    	_lock();
    	
    	try
    	{
            //首次结算
            if(_m_lLastSettleTimeMs <= 0)
            {
                _m_lLastSettleTimeMs = CommonFunc.getNowTimeMS();
                //推送数据
                if(!_isInit)
                {
                    getComp().getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_055_OnMarsEnergyChg(this));
                }
                return;
            }

            //计算时长，不满1分钟不予处理
            long nowTimeMs = CommonFunc.getNowTimeMS();
            long planSettleMins = (nowTimeMs - _m_lLastSettleTimeMs) / 60000;
            if(planSettleMins < 1)
                return;

			long preCount = _m_lCount;

            //开始结算消耗数量
    		do
    		{
    			//无产出速度，调整到最近的分钟结算时刻
    			if(_m_lCostSpeed <= 0)
                    break;

    			//当前没有产量，调整到最近的分钟结算时刻
    			if(_m_lCount <= 0)
                    break;

    			//计算可以消耗的时长
    			long planSettleCount = planSettleMins * _m_lCostSpeed;
    			if(_m_lCount > planSettleCount) //库存有剩余
                {
                    _m_lCount = _m_lCount - planSettleCount;
                }
                else //消耗完成
                {
                    _m_lCount = 0;
                }

    		} while(false);

            _m_lLastSettleTimeMs += planSettleMins * 60000;
    		_saveData();
    		
    		//数据出现变化再进行推送（且非初始化）
    		if(!_isInit)
    		{
    			getComp().getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_055_OnMarsEnergyChg(this));
    		}
    		
    		//日志数据
    		if(preCount != _m_lCount)
    		{
    			_log((_m_lCount - preCount), NPPlayerContext.createNew(ENPGameEvent.MARS_ENERGY_SETTLE));
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    public long getItemCount()
    {
        _lock();
        
        try
        {
            //进行结算
            _doSettle(false);

            return _m_lCount;
        } 
        finally
        {
            _unlock();
        }
    }
    
    public boolean hasItem(long _count)
    {
        _lock();
        
        try
        {
            //进行结算
            _doSettle(false);
            
            return _m_lCount >= _count;
        } 
        finally
        {
            _unlock();
        }
    }
    
    /**
     * 设置数量
     * @param _count
     * @param _context
     */
    public void setItemCount(long _count, NPPlayerContext _context)
    {
        _lock();
        
        try
        {
        	if(_count == _m_lCount)
        		return;
        	
        	_m_lCount = _count;
            _saveData();

            //通知客户端
            getComp().getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_055_OnMarsEnergyChg(this));
            
            //日志数据
            _log(0, _context);
        } 
        finally
        {
            _unlock();
        }
    }
    
    public void gainItem(long _count, boolean _needPush, boolean _isNotMerge, NPPlayerContext _context)
    {
        _lock();
        
        try
        {
            //进行结算
            _doSettle(false);

            //增加金币
            _m_lCount += _count;
            //增加总获取数值
            _m_lTotalGainCount += _count;
            //保存数据
            _saveData();

            //通知客户端
            if (_needPush)
                getComp().getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_055_OnMarsEnergyChg(this));
            
            //回包数据
            _context.collectItem(ENPItemType.CURRENCY, ECurrency.MARS_ENERGY.ordinal(), _count, _isNotMerge);
            
            //日志数据
            _log(_count, _context);
        } 
        finally
        {
            _unlock();
        }
    }
    
    public boolean spendItem(long _count, NPPlayerContext _context)
    {
        _lock();
        
        try
        {
            //进行结算
            _doSettle(false);

            //消耗金币
            if (_count > _m_lCount)
                return false;

            //扣除金币
            _m_lCount -= _count;
            //累计消耗数值总值
            _m_lTotalConsumeCount += _count;
            //保存数据
            _saveData();

            //通知客户端
            getComp().getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_055_OnMarsEnergyChg(this));
            
            //日志数据
            _log(-_count, _context);

            return true;
        } 
        finally
        {
            _unlock();
        }
    }
    
    /**
     * 构造协议数据
     * @return
     */
    public Player_MarsEnergyInfo toProto()
    {
        Player_MarsEnergyInfo proto = new Player_MarsEnergyInfo();
        proto.setLastSettleTimeMs(_m_lLastSettleTimeMs);
        proto.setCount(_m_lCount);
        proto.setCostSpeed(_m_lCostSpeed);
        
        return proto;
    }
    
    /**
     * 日志数据
     * @param _chgValue
     * @param _context
     */
    private void _log(long _chgValue, NPPlayerContext _context)
    {
    	BM bmObj = getUserData().getUSServer().getBM();
    	
    	LogCurrencyMarsEnergyBO logBo = new LogCurrencyMarsEnergyBO();
    	logBo.setCid(bmObj, getUserData().getCid());
    	logBo.setPlayerLevel(bmObj, (int) getUserData().getParam(ENPPlayerParam.LEVEL));
    	logBo.setValue(bmObj, _m_lCount);
    	logBo.setCostSpeed(bmObj, _m_lCostSpeed);
    	logBo.setLastSettleTimeMs(bmObj, _m_lLastSettleTimeMs);
    	logBo.setTotalGainCount(bmObj, _m_lTotalGainCount);
    	logBo.setTotalConsumeCount(bmObj, _m_lTotalConsumeCount);
    	
    	CommLogDB.log(bmObj, logBo, _context);
    }
}
