package NPUSServer.NPUSUserMgr.UserComp.RecordComp;

import Common.ActivityEnum.EActivityState;
import Common.NpPlayerInfoObj.PlayerInfo_Record;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.Refs.Activity.RefActivity;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_MAX_POWER_UP;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer.NPRecordExtDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerRecordBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


/****************************
 * 玩家属性的一部分
 * @author Administrator
 *
 *
 */
public class PlayerRecordComponent extends _ANPUserComponent implements _IUserItemBasicDealer, _IHandlerHolder
{
    //玩家记录数据对象
    private HashMap<Integer, PlayerRecordInfo> _m_hmRecordMap;
    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;

    public PlayerRecordComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.RECORD_COMP);

        _m_hmRecordMap = new HashMap<>();

        //玩家属性修改器
        _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer(getClass().getName());
    }

    public NPPlayerPropertyContainer getPlayerPropertyContainer()
    {
        return _m_pcPlayerPropertyContainer;
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerRecordBO>>()
                {
                    @Override
                    public void dealFail()
                    {
                        USLog.error(getUSServer(), "Can not load PlayerRecordBO[cid:" + getUserData().getCid() + "]");

                        getUserData().setDataLoadFail();
                    }

                    @Override
                    public void dealSuc(List<PlayerRecordBO> _list)
                    {
                        _initBoList(_list);
                    }
                });
    }

    private void _initBoList(List<PlayerRecordBO> _boList)
    {
        for (PlayerRecordBO bo : _boList)
        {
            PlayerRecordInfo info = new PlayerRecordInfo(this, bo);
            _m_hmRecordMap.put(bo.getType(), info);
        }

        setInited();
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        getUSServer().getCommActivityMgr().activityStateChg.addHandler(this, new HandlerTwo<_AActivityBase, EActivityState>()
        {
            @Override
            public void handle(_AActivityBase _activity, EActivityState _state)
            {
                getUserData().lockUser();
                try
                {
                    _checkNeedTriggerMarsPowerEvent(_activity.getActivityId(), _state);
                } finally
                {
                    getUserData().unlockUser();
                }
            }
        });

        for (_AActivityBase activity : getUSServer().getCommActivityMgr().getAllActivity())
        {
            _checkNeedTriggerMarsPowerEvent(activity.getActivityId(), activity.getCurState().getStateType());
        }
    }

    /**
     * 检查是否需要触发火星最大战力提升事件
     * @param _activityId
     * @param _state
     */
    private void _checkNeedTriggerMarsPowerEvent(Long _activityId, EActivityState _state)
    {
        long recordCount = getRecordCount(ENPPlayerRecordParam.MARS_MAX_POWER);
        if (recordCount <= 0)
            return;

        RefActivity refActivity = RefActivity.getMgr().get(_activityId);
        if (refActivity != null && _state == EActivityState.PLAYING)
        {
            boolean needTrigger = false;

            List<Long> MarsPowerRankIdList = RefGeneral.Ref().logicEventRankMap.get(Event_P_MARS_MAX_POWER_UP.ID);
            if (MarsPowerRankIdList == null || MarsPowerRankIdList.isEmpty())
            {
                USLog.warn(getUSServer(), "MarsPowerRankIdList is empty, can not trigger Event_P_MARS_MAX_POWER_UP");
                return;
            }

            for (Long rankId : refActivity.rank_id_list)
            {
                if (MarsPowerRankIdList.contains(rankId))
                {
                    needTrigger = true;
                    break;
                }
            }

            if (needTrigger)
            {
                getUserData().onLogicEvent(
                        new Event_P_MARS_MAX_POWER_UP(NPPlayerContext.createNew(ENPGameEvent.NONE),
                                0, recordCount));
            }
        }
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        getUSServer().getCommActivityMgr().activityStateChg.clear(this);
    }

    /****************** ItemDealer处理部分 ******************/
    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.RECORD;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        ENPPlayerRecordParam recordParam = ENPPlayerRecordParam.ENPPlayerRecordParam_FromInt((int) _itemId);
        if (null == recordParam)
        {
            return 0;
        }

        return getRecordCount(recordParam);
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        ENPPlayerRecordParam recordParam = ENPPlayerRecordParam.ENPPlayerRecordParam_FromInt((int) _itemId);
        if (null == recordParam)
        {
            return false;
        }

        return getRecordCount(recordParam) >= _count;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        ENPPlayerRecordParam recordParam = ENPPlayerRecordParam.ENPPlayerRecordParam_FromInt((int) _itemId);
        if (null == recordParam)
        {
            USLog.error(getUSServer(), "gainItem,record error, record:{}", _itemId);
            return;
        }

        addRecord(recordParam, _count, _context);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    /****************** ItemDealer处理部分 ******************/

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    /**
     * 构造协议对象
     * @param _list
     */
    public void makeProto(ArrayList<PlayerInfo_Record> _list)
    {
        getUserData().lockUser();

        try
        {
            for (PlayerRecordInfo record : _m_hmRecordMap.values())
            {
                if (null == record)
                    continue;

                _list.add(record.toProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取指定记录类型的计数
     * @param _recordParam
     * @return
     */
    public long getRecordCount(ENPPlayerRecordParam _recordParam)
    {
        getUserData().lockUser();

        try
        {
            PlayerRecordInfo info = _m_hmRecordMap.get(_recordParam.ordinal());

            return null == info ? 0 : info.getCount();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 确认玩家记录对象并修改计数
     * @param _recordParam
     * @param _chgCount
     * @param _context
     */
    public void ensureRecord(ENPPlayerRecordParam _recordParam, long _chgCount, ENCounterDealType _dealType, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            PlayerRecordInfo info = _m_hmRecordMap.get(_recordParam.ordinal());
            if (null == info)
            {
                BM bmObj = getUSServer().getBM();

                PlayerRecordBO bo = new PlayerRecordBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setType(bmObj, _recordParam.ordinal());
                bo.setCount(bmObj, _chgCount);
                bo.insert(bmObj);

                info = new PlayerRecordInfo(this, bo);
                _m_hmRecordMap.put(bo.getType(), info);

                //后续操作
                NPRecordExtDealerMgr.getInstance().dealOnCountChg(getUserData(), _recordParam, 0, _chgCount, _context);
            } else
            {
                info.chgCount(_chgCount, _dealType, _context);
            }

            //推送协议
            getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_060_OnPlayerRecordChg(info.toProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /********* 快捷处理方式 *********/
    public void addRecord(ENPPlayerRecordParam _recordParam, long _count, NPPlayerContext _context)
    {
        ensureRecord(_recordParam, _count, ENCounterDealType.ADD, _context);
    }

    public void reduceRecord(ENPPlayerRecordParam _recordParam, long _count, NPPlayerContext _context)
    {
        ensureRecord(_recordParam, _count, ENCounterDealType.REDUCE, _context);
    }

    public void setRecord(ENPPlayerRecordParam _recordParam, long _count, NPPlayerContext _context)
    {
        ensureRecord(_recordParam, _count, ENCounterDealType.SET, _context);
    }

    public void setGtRecord(ENPPlayerRecordParam _recordParam, long _count, NPPlayerContext _context)
    {
        ensureRecord(_recordParam, _count, ENCounterDealType.SET_GT, _context);
    }

    @Override
    public String toString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.append("NPPlayerRecordComponent record info:").append("\n");
        for (PlayerRecordInfo recordInfo : _m_hmRecordMap.values())
        {
            stringBuilder.append(recordInfo.getType()).append(":").append(recordInfo.getCount()).append("\n");
        }
        return stringBuilder.toString();
    }
}
