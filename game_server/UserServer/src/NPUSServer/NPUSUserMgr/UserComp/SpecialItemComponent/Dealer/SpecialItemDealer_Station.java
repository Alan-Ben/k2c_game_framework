package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.PlayerObj.Player_StationInfo;
import Common.ServerObj.ServerObj_SpecialItem_Station;
import CommonEnum.ESpecialItemType;
import GS2GC.p004_PlayerOp.GS2GC_004_057_OnStationInfoChg;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Arena.RefArenaStationLevel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent._ASpecialItemDealer;
import NPUSServer.USLog;
import USDB.Bo.PlayerSpecialItemBO;

import java.nio.ByteBuffer;

public class SpecialItemDealer_Station extends _ASpecialItemDealer implements _IHandlerHolder
{
    private long _m_dbId;
    private int _m_level;
    private long _m_lastSettleTimeMs;
    private long _m_hadOutputNum;//已产出数量
    private long _m_hadOutputSec;//已产出时间
    private boolean _m_isUnlimited;//是否是无限制存储
    private RefArenaStationLevel _m_refLevel;//等级配置

    private LazyTaskDealer _m_lazyDealer;
    //保存数据到数据库的处理，减少数据库操作的次数
    private LazyTaskDealer _m_lazySaveDealer;

    public SpecialItemDealer_Station(SpecialItemComponent _comp)
    {
        super(_comp);

        _m_dbId = 0;
        _m_level = 0;
        _m_lastSettleTimeMs = 0;
        _m_hadOutputNum = 0;
        _m_hadOutputSec = 0;
        _m_isUnlimited = false;
        _m_refLevel = null;

        //延迟处理器
        _m_lazyDealer = new LazyTaskDealer(() -> _doSettle(false, true), 100);
        _m_lazySaveDealer = new LazyTaskDealer(this::_dealUpdateSave, 500);
    }

    @Override
    public ESpecialItemType getSpecialItemType()
    {
        return ESpecialItemType.ARENA_STATION;
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
        
        //监听权限变化
        getComp().getUserData().getPlayerPermissionsComponent().getPermissionChgDelegate().addHandler(this, 
        		new HandlerOne<Long>() 
        {
			@Override
			public void handle(Long _permissionId) 
			{
				if(RefGeneral.Ref().arena_station_privilege_permissions_id == _permissionId)
				{
					doDelaySettle();
				}
			}
		});
    }

    @Override
    public void dispose()
    {
    	getComp().getUserData().getPlayerComponent().getEarningsChgDelegate().clear(this);
    	getComp().getUserData().getPlayerPermissionsComponent().getPermissionChgDelegate().clear(this);
    }

    @Override
    public void initBo(PlayerSpecialItemBO _bo)
    {
        _m_dbId = _bo.getId();

        if (_bo.getData() != null)
        {
            ServerObj_SpecialItem_Station proto = new ServerObj_SpecialItem_Station();
            proto.readPackage(ByteBuffer.wrap(_bo.getData()));

            _m_lastSettleTimeMs = proto.getLastSettleTimeMs();
            _m_level = proto.getLevel();
            _m_hadOutputNum = proto.getHadOutputNum();
            _m_hadOutputSec = proto.getHadOutputSec();
            _m_isUnlimited = proto.getIsUnlimited();

            _m_refLevel = RefArenaStationLevel.getMgr().get(_m_level);
            if (_m_refLevel == null)
                USLog.error(getComp().getUSServer(), "SpecialItemDealer_Station initBo refLevel is null, cid:{} level:{}",
                        getComp().getUserData().getCid(), _m_level);
        }
    }

    /**
     * 获取村庄产速
     */
    public long getEarnings()
    {
        return getComp().getUserData().getPlayerComponent().getEarnings();
    }

    /**
     * 执行延迟结算
     */
    public void doDelaySettle()
    {
        _m_lazyDealer.setNeedDeal();
    }

    /**
     * 解锁功能
     */
    public void unlockFunc()
    {
        _lock();
        try{
            //如果已经解锁则不处理
            if (_m_dbId != 0)
                return;

            //查询配置
            RefArenaStationLevel refArenaStationLevel = RefArenaStationLevel.getMgr().get(1);
            if (refArenaStationLevel == null)
            {
                USLog.error(getComp().getUSServer(), "SpecialItemDealer_Station unlockFunc refArenaStationLevel not found");
                return;
            }

            _m_refLevel = refArenaStationLevel;

            _m_level = 1;
            _m_hadOutputSec = refArenaStationLevel.storage_limit_sec;
            long speed = (long) Math.ceil(getEarnings() * _m_refLevel.harvest_ratio / 10000d);
            _m_hadOutputNum = _m_hadOutputSec * speed;
            _m_lastSettleTimeMs = CommonFunc.getNowTimeMS();

            //如果未保存数据库则需要马上插入，如果已经保存则处理更新行为
            BM bmObj = getComp().getUSServer().getBM();
            //构造保存数据
            ServerObj_SpecialItem_Station proto = new ServerObj_SpecialItem_Station();
            proto.setLevel(_m_level);
            proto.setHadOutputNum(_m_hadOutputNum);
            proto.setHadOutputSec(_m_hadOutputSec);
            proto.setLastSettleTimeMs(_m_lastSettleTimeMs);

            PlayerSpecialItemBO bo = new PlayerSpecialItemBO();
            bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
            bo.setType(getComp().getUSServer().getBM(), getSpecialItemType().ordinal());
            bo.setData(getComp().getUSServer().getBM(), CommonFunc.ByteBfferToBytes(proto.makePackage()));
            bo.insert(bmObj);

            _m_dbId = bo.getId();

            getComp().getUserData().sendMsgToGC(new GS2GC_004_057_OnStationInfoChg(toProto()));
        }finally
        {
            _unlock();
        }
    }

    /**
     * 结算
     */
    private void _doSettle(boolean _fullFill, boolean _needPush)
    {
        _lock();
        try
        {
            //如果未解锁则不处理
            if (_m_dbId == 0)
                return;

            //如果等级配置为空则不处理
            if (_m_refLevel == null)
            {
                USLog.error(getComp().getUSServer(), "SpecialItemDealer_Station _doSettle refLevel is null, cid:{} level:{}",
                        getComp().getUserData().getCid(), _m_level);
                return;
            }

            //计算还可以存储的时间
            long canStoreSec = 0;
            if(_fullFill) //需要自动补充：只能填满当前等级允许的上限
            {
            	canStoreSec = _m_refLevel.storage_limit_sec - _m_hadOutputSec;
            }
            else //需要考虑无上限情况
            {
            	long limitSecs = _m_isUnlimited ? RefGeneral.Ref().arena_station_privilege_collect_limit_time_sec : _m_refLevel.storage_limit_sec;
            	canStoreSec = limitSecs - _m_hadOutputSec;
            }
            
            //结算逻辑
            long nowTimeMS = CommonFunc.getNowTimeMS();
            long durationSec = (nowTimeMS - _m_lastSettleTimeMs) / 1000;

            //实际产出时间
            long realOutputSec = _fullFill ? canStoreSec : Math.min(durationSec, canStoreSec);

            //实际产出数量
            if (realOutputSec > 0)
            {
            	//产出速度
            	long speed = (long) Math.ceil(getEarnings() * _m_refLevel.harvest_ratio / 10000d);
            	//计算产出数量
            	long canOuputNum = RefGeneral.Ref().arena_station_privilege_collect_limit_num - _m_hadOutputNum;
            	if(canOuputNum > 0) //还有可以存储的空间
            	{
            		long canOuputSecs = canOuputNum / speed;
            		if(realOutputSec >= canOuputSecs) //超过上限
            		{
            			_m_hadOutputSec += canOuputSecs;
            			_m_hadOutputNum = canOuputNum;
            		}
            		else
            		{
                    	_m_hadOutputSec += realOutputSec;
                        _m_hadOutputNum += realOutputSec * speed;
            		}
            	}
            }

            _m_lastSettleTimeMs = nowTimeMS;
            //是否是无限制存储
            _m_isUnlimited = getComp().getUserData().getPlayerPermissionsComponent().checkPermissionEffect(RefGeneral.Ref().arena_station_privilege_permissions_id);
            //更新等级配置
            if (_m_level != _m_refLevel.level)
            {
                _m_refLevel = RefArenaStationLevel.getMgr().get(_m_level);
                if (_m_refLevel == null)
                    USLog.error(getComp().getUSServer(), "SpecialItemDealer_Station _doSettle refLevel is null, cid:{} level:{}",
                            getComp().getUserData().getCid(), _m_level);
            }

            //保存数据
            _saveData();

            //通知客户端
            if (_needPush)
                getComp().getUserData().sendMsgToGC(new GS2GC_004_057_OnStationInfoChg(toProto()));

        } finally
        {
            _unlock();
        }
    }

    /**
     * 收集产出
     */
    public Result collect(NPPlayerContext _context)
    {
        _lock();
        try
        {
            //如果未解锁则不处理
            if (_m_dbId == 0)
                return CommErr.SYSTEM_UNLOCK;

            //先进行结算
            _doSettle(false, false);

            //如果没有产出则不处理
            if (_m_hadOutputSec <= 0)
                return PlayerErr.STATION_IS_EMPTY;

            //计算收集数量
            long collectNum = _m_hadOutputNum;
            _m_hadOutputNum = 0;
            _m_hadOutputSec = 0;

            //保存数据
            _saveData();

            //通知客户端
            getComp().getUserData().sendMsgToGC(new GS2GC_004_057_OnStationInfoChg(toProto()));

            //增加金币
            getComp().getUserData().gainItem(RefGeneral.Ref().arena_station_output_item.getItemType(),
                    RefGeneral.Ref().arena_station_output_item.getItemId(), collectNum, _context);

            getComp().getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.ARENA_STATION_COLLECT_TIMES, 1, _context);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 升级
     */
    public Result upgrade(NPPlayerContext _context)
    {
        _lock();
        try
        {
            //如果未解锁则不处理
            if (_m_dbId == 0)
                return CommErr.SYSTEM_UNLOCK;

            if (_m_refLevel == null)
            {
                USLog.error(getComp().getUSServer(), "SpecialItemDealer_Station upgrade refLevel is null, cid:{} level:{}",
                        getComp().getUserData().getCid(), _m_level);
                return CommErr.REF_NOT_FOUND;
            }

            //查询下一等级配置
            RefArenaStationLevel newLevelRef = RefArenaStationLevel.getMgr().get(_m_level + 1);
            if (newLevelRef == null)
                return PlayerErr.STATION_MAX_LEVEL;

            //计算升级需要的金币
            if (!getComp().getUserData().spendItem(_m_refLevel.upgrade_cost, _context))
                return CommErr.ITEM_NOT_ENOUGH;

            //更新等级
            _m_level = newLevelRef.level;
            _m_refLevel = newLevelRef;

            //做一次结算
            _doSettle(true, true);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 保存数据
     */
    private void _saveData()
    {
        //保存的处理需要通过LasyDealer处理
        _m_lazySaveDealer.setNeedDeal();
    }

    /********
     * 保存数据库的更新处理，insert需要马上处理
     */
    private void _dealUpdateSave()
    {
        _lock();
        try{
            if (_m_dbId == 0)
                return;

            BM bmObj = getComp().getUSServer().getBM();
            //构造保存数据
            ServerObj_SpecialItem_Station proto = new ServerObj_SpecialItem_Station();
            proto.setLevel(_m_level);
            proto.setHadOutputNum(_m_hadOutputNum);
            proto.setHadOutputSec(_m_hadOutputSec);
            proto.setLastSettleTimeMs(_m_lastSettleTimeMs);
            proto.setIsUnlimited(_m_isUnlimited);

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("data", proto.makePackage());

            bmObj.getBM(PlayerSpecialItemBO.class).update("id", _m_dbId, updateValue);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 构造协议
     * @return
     */
    public Player_StationInfo toProto()
    {
        Player_StationInfo proto = new Player_StationInfo();
        proto.setLastSettleTimeMs(_m_lastSettleTimeMs);
        proto.setLevel(_m_level);
        proto.setHadOutputSec(_m_hadOutputSec);
        proto.setHadOutputNum(_m_hadOutputNum);
        return proto;
    }
}
