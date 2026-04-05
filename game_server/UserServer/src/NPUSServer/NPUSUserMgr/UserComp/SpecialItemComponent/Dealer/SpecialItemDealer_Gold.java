package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.PlayerObj.Player_GoldInfo;
import Common.ServerObj.ServerObj_SpecialItem_Gold;
import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import GS2GC.p004_PlayerOp.GS2GC_004_056_OnGoldInfoChg;
import NPCommon.DB.BM.BM;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerPropertyType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent._ASpecialItemDealer;
import USDB.Bo.PlayerSpecialItemBO;
import USLOGDB.Bo.LogCurrencySilverBO;

import java.nio.ByteBuffer;

public class SpecialItemDealer_Gold extends _ASpecialItemDealer implements _IHandlerHolder
{
    private long _m_dbId;
    private long _m_lastSettleTimeMs;
    private long _m_count;
    private long _m_earnings;//不包括自动点击的收益

    private long _m_offlineOutput;
    private long _m_offlineTimeMs;

    private long _m_lTotalConsumeCount;//总消耗数量

    private LazyTaskDealer _m_lazyDealer;
    //保存数据到数据库的处理，减少数据库操作的次数
    private LazyTaskDealer _m_lazySaveDealer;

    public SpecialItemDealer_Gold(SpecialItemComponent _comp)
    {
        super(_comp);

        _m_dbId = 0;
        _m_lastSettleTimeMs = CommonFunc.getNowTimeMS();
        _m_count = 0;
        _m_earnings = 0;

        _m_lTotalConsumeCount = 0;

        //延迟处理器
        _m_lazyDealer = new LazyTaskDealer(() -> _doSettle(true, false), 100);
        _m_lazySaveDealer = new LazyTaskDealer(this::_dealUpdateSave, 500);
    }

    @Override
    public ESpecialItemType getSpecialItemType()
    {
        return ESpecialItemType.GOLD;
    }

    @Override
    public void initBo(PlayerSpecialItemBO _bo)
    {
        _m_dbId = _bo.getId();

        if (_bo.getData() != null)
        {
            ServerObj_SpecialItem_Gold proto = new ServerObj_SpecialItem_Gold();
            proto.readPackage(ByteBuffer.wrap(_bo.getData()));

            _m_lastSettleTimeMs = proto.getLastSettleTimeMs();
            _m_count = proto.getCount();
            _m_earnings = proto.getOutputSpeed();

            _m_lTotalConsumeCount = proto.getTotalConsumeCount();
        }
    }

    /**
     * 获取金币数量
     * @return
     */
    public long getItemCount()
    {
        _lock();
        try
        {
            //进行结算
            _doSettle(true, false);

            return _m_count;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否有足够的金币
     * @param _count
     * @return
     */
    public boolean hasItem(long _count)
    {
        _lock();
        try
        {
            if (_count <= _m_count)
                return true;

            //进行结算
            _doSettle(true, false);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置金币数量
     * @param _count
     */
    public void setItemCount(long _count, NPPlayerContext _context)
    {
        _lock();
        try
        {
        	if(_m_count == _count)
        		return;
        	
            _m_count = _count;
            _saveData();

            //通知客户端
            getComp().getUserData().sendMsgToGC(new GS2GC_004_056_OnGoldInfoChg(toProto()));

            //日志数据
            _log(0, _context);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加金币
     * @param _count
     * @param _needPush
     * @param _isNotMerge
     * @param _context
     */
    public void gainItem(long _count, boolean _needPush, boolean _isNotMerge, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //进行结算
            _doSettle(false, false);

            //增加金币
            _m_count += _count;
            //保存数据
            _saveData();

            _context.collectItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), _count, _isNotMerge);

            //通知客户端
            if (_needPush)
                getComp().getUserData().sendMsgToGC(new GS2GC_004_056_OnGoldInfoChg(toProto()));

            //日志数据
            _log(_count, _context);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 消耗金币
     * @param _count
     * @param _context
     * @return
     */
    public boolean spendItem(long _count, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //进行结算
            _doSettle(false, false);

            //消耗金币
            if (_count > _m_count)
                return false;

            //扣除金币
            _m_count -= _count;
            //累计消耗数值总值
            _m_lTotalConsumeCount += _count;
            //保存数据
            _saveData();

            //通知客户端
            getComp().getUserData().sendMsgToGC(new GS2GC_004_056_OnGoldInfoChg(toProto()));

            //日志数据
            _log(-_count, _context);
        } finally
        {
            _unlock();
        }

        return true;
    }

    @Override
    public void onInited()
    {
        getComp().getUserData().getPlayerComponent().getEarningsChgDelegate().addHandler(this, new HandlerNone()
        {
            @Override
            public void handle()
            {
                doDelaySettle();
            }
        });

        getComp().getUserData().getPlayerComponent().getAutoTapEarningsChgDelegate().addHandler(this, new HandlerNone()
        {
            @Override
            public void handle()
            {
                doDelaySettle();
            }
        });
    }

    @Override
    public void dispose()
    {
    	getComp().getUserData().getPlayerComponent().getEarningsChgDelegate().clear(this);
    	getComp().getUserData().getPlayerComponent().getAutoTapEarningsChgDelegate().clear(this);
    }

    public long getTotalGainCount()
    {
        _doSettle(false, false);

        return _m_lTotalConsumeCount + _m_count;
    }

    public long getTotalConsumeCount()
    {
        return _m_lTotalConsumeCount;
    }

    public long getOfflineProduceCount()
    {
        return _m_offlineOutput;
    }

    public long getOfflineProduceTime()
    {
        return _m_offlineTimeMs;
    }

    /**
     * 在线状态变更
     * @param _online
     */
    public void onlineStateChg(boolean _online)
    {
        _doSettle(false, _online);
    }

    /**
     * 执行延迟结算
     */
    public void doDelaySettle()
    {
        _m_lazyDealer.setNeedDeal();
    }

    /**
     * 结算
     */
    public void _doSettle(boolean _needPush, boolean _isInit)
    {
        _lock();
        try
        {
            //结算逻辑
            long nowTimeMS = CommonFunc.getNowTimeMS();
            long durationSec = (nowTimeMS - _m_lastSettleTimeMs) / 1000;

            //如果大于1s，按照原有速度结算
            long addCount = 0;
            if (durationSec > 0)
            {
                //如果是离线结算，需要限制结算时间
                if (_isInit)
                    durationSec = Math.min(durationSec, getComp().getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.OFFLINE_OUTPUT_LIMIT_SEC));

                //如果是初始化结算 不计入农田产出
                long speed = _m_earnings + (_isInit ? 0 : getComp().getUserData().getPlayerComponent().getAutoTapEarnings());

                addCount = durationSec * speed;
                _m_count += addCount;
                _m_lastSettleTimeMs = _m_lastSettleTimeMs + durationSec * 1000;

                //离线产出
                if (_isInit)
                {
                    _m_offlineOutput = addCount;
                    _m_offlineTimeMs = durationSec * 1000;
                }
            }

            //检查速度有没有变化
            long preSpeed = _m_earnings;
            long newSpeed = getComp().getUserData().getPlayerComponent().getEarnings();
            if (preSpeed != newSpeed)
            {
                _m_earnings = newSpeed;
            }

            //如果数据变化存储
            if (preSpeed != newSpeed || addCount > 0)
            {
                //保存数据
                _saveData();

                //通知客户端
                if (_needPush)
                    getComp().getUserData().sendMsgToGC(new GS2GC_004_056_OnGoldInfoChg(toProto()));

                //日志数据
                _log(addCount, NPPlayerContext.createNew(ENPGameEvent.GOLD_SETTLE));
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取产出速度
     * 包括赚速+自动点击赚速
     * @return
     */
    private long getTotalSpeed()
    {
        return getComp().getUserData().getPlayerComponent().getEarnings()
                + getComp().getUserData().getPlayerComponent().getAutoTapEarnings();
    }

    /**
     * 保存数据
     */
    private void _saveData()
    {
        //如果未保存数据库则需要马上插入，如果已经保存则处理更新行为
        _lock();
        try
        {
            if (0 == _m_dbId)
            {
                BM bmObj = getComp().getUSServer().getBM();
                //构造保存数据
                ServerObj_SpecialItem_Gold proto = new ServerObj_SpecialItem_Gold();
                proto.setLastSettleTimeMs(_m_lastSettleTimeMs);
                proto.setOutputSpeed(_m_earnings);
                proto.setCount(_m_count);
                proto.setTotalConsumeCount(_m_lTotalConsumeCount);

                PlayerSpecialItemBO bo = new PlayerSpecialItemBO();
                bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                bo.setType(getComp().getUSServer().getBM(), getSpecialItemType().ordinal());
                bo.setData(getComp().getUSServer().getBM(), CommonFunc.ByteBfferToBytes(proto.makePackage()));
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else
            {
                //保存的处理需要通过LasyDealer处理
                _m_lazySaveDealer.setNeedDeal();
            }
        } finally
        {
            _unlock();
        }
    }

    /********
     * 保存数据库的更新处理，insert需要马上处理
     */
    private void _dealUpdateSave()
    {
        _lock();
        try
        {
            if (_m_dbId == 0)
                return;

            BM bmObj = getComp().getUSServer().getBM();
            //构造保存数据
            ServerObj_SpecialItem_Gold proto = new ServerObj_SpecialItem_Gold();
            proto.setLastSettleTimeMs(_m_lastSettleTimeMs);
            proto.setOutputSpeed(_m_earnings);
            proto.setCount(_m_count);
            proto.setTotalConsumeCount(_m_lTotalConsumeCount);

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("data", proto.makePackage());

            bmObj.getBM(PlayerSpecialItemBO.class).update("id", _m_dbId, updateValue);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造协议
     * @return
     */
    public Player_GoldInfo toProto()
    {
        Player_GoldInfo proto = new Player_GoldInfo();
        proto.setLastSettleTimeMs(_m_lastSettleTimeMs);
        proto.setCount(_m_count);
        proto.setOutputSpeed(getTotalSpeed());
        proto.setTotalConsumeCount(_m_lTotalConsumeCount);
        return proto;
    }
    
    /**
     * 通过时长计算获取的金币
     * @param _secs
     * @param _needPush
     * @param _isNotMerge
     * @param _context
     */
    public void gainItemBySecs(int _secs, boolean _needPush, boolean _isNotMerge, NPPlayerContext _context)
    {
    	if(_secs <= 0)
    		return;
    	
    	_lock();
    	
    	try
    	{
    		//先进行结算，同时获取最新的产出速度
    		_doSettle(false, _isNotMerge);
    		
    		//没有产出速度，所以无许计算产出
    		if(_m_earnings <= 0)
    			return;
    		
    		//更新产出数据
    		long addCount = _secs * _m_earnings;
            _m_count += addCount;
            //保存数据
            _saveData();

            //记录获取的物品
            _context.collectItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), addCount, _isNotMerge);

            //通知客户端
            if (_needPush)
            {
            	getComp().getUserData().sendMsgToGC(new GS2GC_004_056_OnGoldInfoChg(toProto()));
            }
            
            //日志数据
            _log(addCount, _context);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 获取最新产速
     * @return
     */
    public long getEarnings()
    {
    	_lock();
    	
    	try
    	{
    		//先进行结算，同时获取最新的产出速度
    		_doSettle(false, false);
    		
    		return _m_earnings;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 日志数据
     * @param _chgValue
     * @param _context
     */
    private void _log(long _chgValue, NPPlayerContext _context)
    {
    	BM bmObj = getUserData().getUSServer().getBM();
    	
    	LogCurrencySilverBO logBo = new LogCurrencySilverBO();
    	logBo.setCid(bmObj, getUserData().getCid());
    	logBo.setPlayerLevel(bmObj, (int) getUserData().getParam(ENPPlayerParam.LEVEL));
    	logBo.setValue(bmObj, _m_count);
    	logBo.setOutputSpeed(bmObj, _m_earnings);
    	logBo.setLastSettleTimeMs(bmObj, _m_lastSettleTimeMs);
    	logBo.setTotalConsumeCount(bmObj, _m_lTotalConsumeCount);
    	
    	CommLogDB.log(bmObj, logBo, _context);
    }
}
