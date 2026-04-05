package NPUSServer.CommonActivityMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ActivityEnum.EActivityState;
import Common.ActivityObj.Activity_Info;
import Common.ActivityObj.Activity_PlayerData;
import Common.ActivityObj.Activity_StepRewardInfo;
import CommonEnum.ECommonActivityType;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateTwo;
import NPGameRes.Refs.Activity.RefActivity;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.StepReward.ActivityStepRewardInfo;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.CommonActivityMgr.Factory.CommonActivityFactory;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UsActivityScheduleMgr.UsActivityScheduleInfo;
import USDB.Bo.*;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;

/*************************
 * 活动实例对象管理
 *
 */
public class CommonActivityMgr
{
    private NPUserServer _m_usUSServer;
    //活动实例Map，索引-活动ID
    private HashMap<Long, _AActivityBase> _m_hmActivityMap;
    //活动实例列表，用于tick等遍历操作
    private ArrayList<_AActivityBase> _m_alActivityList;
    //每秒tick任务对象
    private CommonActivityTickTask _m_ttActivityTick;
    //锁对象
    private MutexObject _m_mutex;
    //活动状态变更触发器
    public ADelegateTwo<_AActivityBase, EActivityState> activityStateChg;

    public CommonActivityMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_hmActivityMap = new HashMap<>();
        _m_alActivityList = new ArrayList<>();

        _m_ttActivityTick = new CommonActivityTickTask(this);

        _m_mutex = new MutexObject();

        activityStateChg = new ADelegateTwo<>(this);
    }

    public NPUserServer getUSServer()
    {
        return _m_usUSServer;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public List<_AActivityBase> getAllActivity()
    {
        _lock();
        try{
            return new ArrayList<>(_m_alActivityList);
        }finally
        {
            _unlock();
        }
    }

    /**********************
     * 从数据库中加载活动数据
     * @return
     */
    public boolean initFromDB()
    {
        //活动基础数据
        List<ActivityBaseBO> boList = getUSServer().getBM().getBM(ActivityBaseBO.class).s_findAll();
        if (null == boList)
        {
            return false;
        }
        for (int i = 0; i < boList.size(); i++)
        {
            ActivityBaseBO bo = boList.get(i);
            if (null == bo)
                continue;

            //检查是否重复活动
            if (_m_hmActivityMap.containsKey(bo.getId()))
            {
                USLog.error(_m_usUSServer, "activity:{} intanceId:{} init error, mutil activity.", bo.getActivityId(), bo.getId());
                continue;
            }

            RefActivity ref = RefActivity.getMgr().get(bo.getActivityId());
            if (null == ref)
            {
                USLog.error(_m_usUSServer, "activity:{} init error, not find ref.", bo.getActivityId());
                continue;
            }

            _AActivityBase activity = CommonActivityFactory.getInstance().createInstance(getUSServer(), bo);
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity:{} init error, create type:{} fail.", bo.getActivityId(), ref.type_id);
                continue;
            }

            UsActivityScheduleInfo scheduleInfo = getUSServer().getUsActivityScheduleMgr().lookupScheduleByActivityInstanceId(bo.getId());
            if (scheduleInfo == null)
            {
                USLog.error(_m_usUSServer, "activity:{} init error, associate schedule not found ", bo.getActivityId());
                continue;
            }

            if (!activity.s_init(scheduleInfo))
            {
                USLog.error(_m_usUSServer, "activity:{} instanceId:{} init fail.", bo.getActivityId(), bo.getId());
                continue;
            }

            _m_hmActivityMap.put(activity.getInstanceId(), activity);
            _m_alActivityList.add(activity);
        }

        //活动排行榜数据
        List<ActivityRankBO> rankBoList = getUSServer().getBM().getBM(ActivityRankBO.class).s_findAll();
        if (null == rankBoList)
        {
            return false;
        }
        for (ActivityRankBO bo : rankBoList)
        {
            if (null == bo)
                continue;

            _AActivityBase activity = lookupActivity(bo.getInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity rank:{} init fail, not find activity instance:{}", bo.getRankId(), bo.getInstanceId());
                continue;
            }

            activity.initRank(bo);
        }
        //活动排行榜奖励领取记录数据
        List<ActivityRankRewardInfoBO> rankRewardDrawRecordBOList = getUSServer().getBM().getBM(ActivityRankRewardInfoBO.class).s_findAll();
        if (null == rankRewardDrawRecordBOList)
        {
            return false;
        }
        for (ActivityRankRewardInfoBO recordBo : rankRewardDrawRecordBOList)
        {
            if (null == recordBo)
                continue;

            _AActivityBase activity = lookupActivity(recordBo.getActivityInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity rank reward record init fail, not find activity instance:{}", recordBo.getActivityInstanceId());
                continue;
            }

            activity.initRankRewardDrawRecord(recordBo);
        }

        //活动阶段奖励数据
        List<ActivityStepRewardBO> stepRewardBoList = getUSServer().getBM().getBM(ActivityStepRewardBO.class).s_findAll();
        if (null == stepRewardBoList)
        {
            return false;
        }
        for (ActivityStepRewardBO bo : stepRewardBoList)
        {
            if (null == bo)
                continue;

            _AActivityBase activity = lookupActivity(bo.getInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity stepReward:{} init fail, not find activity instance:{}", bo.getStepRewardId(), bo.getInstanceId());
                continue;
            }

            activity.initStepReward(bo);
        }
        //活动排行榜奖励领取记录数据
        List<ActivityStepRewardDrawRecordBO> stepRewardDrawRecordBOList = getUSServer().getBM().getBM(ActivityStepRewardDrawRecordBO.class).s_findAll();
        if (null == stepRewardDrawRecordBOList)
        {
            return false;
        }
        for (ActivityStepRewardDrawRecordBO recordBo : stepRewardDrawRecordBOList)
        {
            if (null == recordBo)
                continue;

            _AActivityBase activity = lookupActivity(recordBo.getActivityInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity step reward record init fail, not find activity instance:{}", recordBo.getActivityInstanceId());
                continue;
            }

            activity.initStepRewardDrawRecord(recordBo);
        }
        //活动排行榜奖励邮件补发记录数据
        List<ActivityStepRewardMailRecordBO> stepRewardMailRecordBOList = getUSServer().getBM().getBM(ActivityStepRewardMailRecordBO.class).s_findAll();
        if (null == stepRewardMailRecordBOList)
        {
            return false;
        }
        for (ActivityStepRewardMailRecordBO recordBo : stepRewardMailRecordBOList)
        {
            if (null == recordBo)
                continue;

            _AActivityBase activity = lookupActivity(recordBo.getActivityInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity step mail record init fail, not find activity instance:{}", recordBo.getActivityInstanceId());
                continue;
            }

            activity.initStepRewardMailRecord(recordBo);
        }
        //活动玩家商店数据
        List<ActivityPlayerShopInfoBO> playerShopBoList = getUSServer().getBM().getBM(ActivityPlayerShopInfoBO.class).s_findAll();
        if (null == playerShopBoList)
        {
            return false;
        }
        for (ActivityPlayerShopInfoBO playerShopBO : playerShopBoList)
        {
            if (null == playerShopBO)
                continue;

            _AActivityBase activity = lookupActivity(playerShopBO.getInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity player shop info init fail, not find activity instance:{}", playerShopBO.getInstanceId());
                continue;
            }

            activity.getShopMgr().initShopInfoFromDB(playerShopBO);
        }
        //活动玩家商店购买记录数据
        List<ActivityPlayerShopBuyRecordBO> playerShopBuyRecordBoList = getUSServer().getBM().getBM(ActivityPlayerShopBuyRecordBO.class).s_findAll();
        if (null == playerShopBuyRecordBoList)
        {
            return false;
        }
        for (ActivityPlayerShopBuyRecordBO playerShopBuyRecordBO : playerShopBuyRecordBoList)
        {
            if (null == playerShopBuyRecordBO)
                continue;

            _AActivityBase activity = lookupActivity(playerShopBuyRecordBO.getInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity player shop buy record init fail, not find activity instance:{}", playerShopBuyRecordBO.getInstanceId());
                continue;
            }

            activity.getShopMgr().initShopBuyRecordFromDB(playerShopBuyRecordBO);
        }
        //活动玩家钻石礼包数据
        List<ActivityPlayerCrystalGiftPackInfoBO> playerCrystalGiftPackBoList = getUSServer().getBM().getBM(ActivityPlayerCrystalGiftPackInfoBO.class).s_findAll();
        if (null == playerCrystalGiftPackBoList)
        {
            return false;
        }
        for (ActivityPlayerCrystalGiftPackInfoBO playerCrystalGiftPackBO : playerCrystalGiftPackBoList)
        {
            if (null == playerCrystalGiftPackBO)
                continue;

            _AActivityBase activity = lookupActivity(playerCrystalGiftPackBO.getInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity player CrystalGiftPack info init fail, not find activity instance:{}", playerCrystalGiftPackBO.getInstanceId());
                continue;
            }

            activity.getCrystalGiftPackMgr().initInfoFromDB(playerCrystalGiftPackBO);
        }
        //活动玩家商店钻石礼包数据
        List<ActivityPlayerCrystalGiftPackBuyRecordBO> playerCrystalGiftPackBuyRecordBoList = getUSServer().getBM().getBM(ActivityPlayerCrystalGiftPackBuyRecordBO.class).s_findAll();
        if (null == playerCrystalGiftPackBuyRecordBoList)
        {
            return false;
        }
        for (ActivityPlayerCrystalGiftPackBuyRecordBO playerCrystalGiftPackBuyRecordBO : playerCrystalGiftPackBuyRecordBoList)
        {
            if (null == playerCrystalGiftPackBuyRecordBO)
                continue;

            _AActivityBase activity = lookupActivity(playerCrystalGiftPackBuyRecordBO.getInstanceId());
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "activity player CrystalGiftPack buy record init fail, not find activity instance:{}", playerCrystalGiftPackBuyRecordBO.getInstanceId());
                continue;
            }

            activity.getCrystalGiftPackMgr().initBuyRecordFromDB(playerCrystalGiftPackBuyRecordBO);
        }

        //按照实例id排序
        _m_alActivityList.sort(Comparator.comparingLong(_AActivityBase::getInstanceId));

        //服务器加载完成后开启每秒tick
        getUSServer().getLoaderMgr().safeCall(() ->
        {
            ALSynTaskManager.getInstance().regTask(_m_ttActivityTick);
        });

        return true;
    }

    /***********************************
     * 注册新活动，创建失败，需要外部手动或其他系统处理
     * @param _scheduleInfo
     * @param _activityId
     * @param _crossInstanceId
     * @param _gameLogicInstanceId
     * @param _startMs
     * @param _endMs
     * @param _closeMs
     * @param _context
     * @return
     */
    public _AActivityBase register(UsActivityScheduleInfo _scheduleInfo, long _activityId, long _crossInstanceId, long _gameLogicInstanceId
            , long _startMs, long _endMs, long _closeMs, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //检查对应配置
            RefActivity ref = RefActivity.getMgr().get(_activityId);
            if (null == ref)
            {
                USLog.error(_m_usUSServer, "CommActivityMgr register ref not found, activity:{} scheduleId:{}", _activityId, _scheduleInfo.getScheduleId());
                return null;
            }

            BM bmObj = getUSServer().getBM();

            ActivityBaseBO bo = new ActivityBaseBO();
            bo.setActivityId(bmObj, ref.activity_id);
            bo.setCrossInstanceId(bmObj, _crossInstanceId);
            bo.setGameLogicInstanceId(bmObj, _gameLogicInstanceId);
            bo.setStartMs(bmObj, _startMs);
            bo.setEndMs(bmObj, _endMs);
            bo.setSettleMs(bmObj, _calSettleTimeMs(_endMs, _closeMs));
            bo.setCloseMs(bmObj, _closeMs);
            bo.setCurState(bmObj, EActivityState.PLAN.ordinal());
            bo.insert(bmObj);

            //创建活动对应处理对象
            _AActivityBase activity = CommonActivityFactory.getInstance().createInstance(getUSServer(),  bo);
            if (null == activity)
            {
                USLog.error(_m_usUSServer, "CommActivityMgr register create activity instance fail, activity:{} scheduleId:{}", _activityId, _scheduleInfo.getScheduleId());
                bo.del(getUSServer().getBM());
                return null;
            }

            //初始化活动对象，如果失败需移除BO，需要手动处理
            if (!activity.initNew(_scheduleInfo))
            {
                USLog.error(_m_usUSServer, "CommActivityMgr register initNew fail, activity:{} scheduleId:{}", _activityId, _scheduleInfo.getScheduleId());
                bo.del(getUSServer().getBM());
                return null;
            }

            _m_hmActivityMap.put(activity.getInstanceId(), activity);
            _m_alActivityList.add(activity);

            //新增活动推送
            onActivityInfoAdd(activity);

            USLog.info(_m_usUSServer, "CommActivityMgr register success, activity:{} scheduleId:{}", _activityId, _scheduleInfo.getScheduleId());

            return activity;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 计算活动结算时间
     * @return
     */
    private static long _calSettleTimeMs(long _endTimeMs, long _closeTimeMs)
    {
        long settleTimeMs = _endTimeMs + RefGeneral.Ref().activity_default_settle_duration_sec * 1000L;
        //结算时间不能超过活动关闭时间
        return settleTimeMs > _closeTimeMs ? _endTimeMs : settleTimeMs;
    }

    /****************************
     * 每秒tick
     * 1. 检查活动状态切换
     * 2. 移除已销毁状态的活动
     */
    public void tick()
    {
        long nowMs = CommonFunc.getNowTimeMS();

        //移除的活动数据列表
        ArrayList<_AActivityBase> discardList = null;

        //更新活动当前状态
        for (_AActivityBase activity : getAllActivity())
        {
            if (null == activity)
                continue;

            //检查活动当前状态
            activity.checkState(nowMs);

            //处理可销毁的活动数据
            if (activity.canDiscard())
            {
                if (null == discardList)
                {
                    discardList = new ArrayList<>();
                }

                discardList.add(activity);
            }
        }

        //移除销毁队列数据
        if (null != discardList)
        {
            for (_AActivityBase activity : discardList)
            {
                //移除活动实例数据
                removeActivity(activity);
                //销毁活动数据
                activity.discard();

                //推送移除
                onActivityInfoRemove(activity.getInstanceId());
            }
        }
    }

    /******************
     * 移除活动实例数据
     * @param _activity
     */
    public void removeActivity(_AActivityBase _activity)
    {
        _lock();
        try
        {
            _m_hmActivityMap.remove(_activity.getInstanceId());
            _m_alActivityList.remove(_activity);
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 查找指定活动对象
     * @param _instanceId
     * @return
     */
    public _AActivityBase lookupActivity(long _instanceId)
    {
        _lock();

        try
        {
            return _m_hmActivityMap.get(_instanceId);
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 查找指定活动对象
     * @param _instanceId
     * @return
     */
    public <T> T lookupActivity(long _instanceId, Class<T> clazz)
    {
        _lock();
        try
        {
            _AActivityBase activity = _m_hmActivityMap.get(_instanceId);
            if (activity == null)
                return null;

            if (clazz.isInstance(activity))
                return clazz.cast(activity);

            return null;
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 获取全部活动对象
     * @return
     */
    public ArrayList<_AActivityBase> lookupAllActivity()
    {
        _lock();

        try
        {
            return new ArrayList<>(_m_alActivityList);
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 获取指定类型的所有活动对象
     * @return
     */
    public List<_AActivityBase> lookupActivityByType(int _type)
    {
        List<_AActivityBase> list = new ArrayList<>();
        _lock();
        try
        {
            for (_AActivityBase _activity : _m_alActivityList)
            {
                if (_activity.getActivityTypeId() == _type)
                {
                    list.add(_activity);
                }
            }
        } finally
        {
            _unlock();
        }
        return list;
    }

    /*******************
     * 获取指定类型的所有活动对象
     * @return
     */
    public <T extends _AActivityBase> T lookupOneActivityByType(ECommonActivityType _type, Class<T> clazz)
    {
        _lock();
        try
        {
            for (_AActivityBase activity : _m_alActivityList)
            {
                if (activity == null)
                    continue;

                if (activity.getActivityTypeId() == _type.ordinal() && clazz.isInstance(activity))
                    return clazz.cast(activity);
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 获取指定类型的所有活动对象
     * @return
     */
    public List<_AActivityBase> lookupActivityByActivityId(long _activityId)
    {
        List<_AActivityBase> list = new ArrayList<>();
        _lock();
        try
        {
            for (_AActivityBase _activity : _m_alActivityList)
            {
                if (_activity.getActivityId() == _activityId)
                {
                    list.add(_activity);
                }
            }
        } finally
        {
            _unlock();
        }
        return list;
    }

    /*******************
     * 获取指定活动id的活动对象
     * @return
     */
    public _AActivityBase lookupOneActivityByActivityId(long _activityId)
    {
        _lock();
        try
        {
            for (_AActivityBase _activity : _m_alActivityList)
            {
                if (_activity.getActivityId() == _activityId)
                {
                    return _activity;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置活动可以下发奖励
     * @param _instanceId 排期id
     */
    public void setCanSendReward(long _instanceId)
    {
        _AActivityBase activity = lookupActivity(_instanceId);
        if (null == activity)
            return;

        activity.setCanSendReward();
    }

    /************
     * 设置活动结束
     * @param _instanceId
     * @param _context
     * @return
     */
    public boolean cmdSettle(long _instanceId, NPPlayerContext _context)
    {
        _lock();

        try
        {
            _AActivityBase activity = lookupActivity(_instanceId);
            if (null == activity)
                return false;

            //判断状态是否为未开启
            if (activity.getCurState().getStateType() != EActivityState.PLAYING)
                return false;

            activity.cmdEnd();

            return true;
        } finally
        {
            _unlock();
        }
    }

    /************
     * 设置活动结束
     * @param _instanceId
     * @param _context
     * @return
     */
    public boolean cmdReward(long _instanceId, NPPlayerContext _context)
    {
        _lock();

        try
        {
            _AActivityBase activity = lookupActivity(_instanceId);
            if (null == activity)
                return false;

            //判断状态是否为未开启
            if (activity.getCurState().getStateType() != EActivityState.SETTLING)
                return false;

            activity.cmdSettle();

            return true;
        } finally
        {
            _unlock();
        }
    }

    /*************
     * 设置活动结束
     * @param _instanceId
     * @param _context
     * @return
     */
    public boolean cmdClose(long _instanceId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            _AActivityBase activity = lookupActivity(_instanceId);
            if (null == activity)
                return false;

            //判断状态是否为未开启
            if (activity.getCurState().getStateType() == EActivityState.PLAN)
                return false;

            activity.cmdClose();

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 移除未开启的活动信息
     * @param _instanceId 活动id
     * @param _context 上下文
     * @return 是否成功
     */
    public boolean cmdRemoveNotStartActivity(long _instanceId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            _AActivityBase activity = lookupActivity(_instanceId);
            if (null == activity)
                return false;

            //判断状态是否为未开启
            if (activity.getCurState().getStateType() != EActivityState.PLAN)
                return false;

            //移除活动对象
            removeActivity(activity);
            //销毁数据
            activity.discard();

            //推送移除
            onActivityInfoRemove(_instanceId);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 新增活动推送
     * @param _activity 活动对象
     */
    public void onActivityInfoAdd(_AActivityBase _activity)
    {
        Activity_Info proto = _activity.toProto();
        ALSynTaskManager.getInstance().regTask(()->{
            getUSServer().getUsUserMgr().broadCastMessage(US2GCWriter_017_ActivityOp.make_050_OnActivityAdd(proto));
        });
    }

    /**
     * 移除活动推送
     * @param _instanceId 活动实例id
     */
    public void onActivityInfoRemove(long _instanceId)
    {
        ALSynTaskManager.getInstance().regTask(()->{
            getUSServer().getUsUserMgr().broadCastMessage(US2GCWriter_017_ActivityOp.make_052_OnActivityRemove(_instanceId));
        });
    }

    /**
     * 活动状态变更监听
     * @param _activity  活动实例
     * @param _stateType 状态类型
     */
    public void onActivityStateChg(_AActivityBase _activity, EActivityState _stateType)
    {
        activityStateChg.onAsyncEvent(_activity, _stateType);
    }

    /**
     * 构造活动列表
     * @return
     */
    public List<Activity_Info> makeProto()
    {
        _lock();
        try{
            List<Activity_Info> list = new ArrayList<>();
            for (_AActivityBase activity : _m_alActivityList)
            {
                list.add(activity.toProto());
            }
            return list;
        }finally{
            _unlock();
        }
    }

    /**
     * 填充玩家数据
     * @param _cid
     * @param _dataList
     */
    public void fillPlayerData(long _cid, List<Activity_PlayerData> _dataList)
    {
        _lock();
        try
        {
            for (_AActivityBase activity : _m_alActivityList)
            {
                activity.fillPlayerData(_cid, _dataList);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过排行实例id查找活动实例
     * @return
     */
    public _AActivityBase lookupActivityByRankInstanceId(long _rankInstanceId)
    {
        _lock();
        try
        {
            for (_AActivityBase activity : _m_alActivityList)
            {
                if (activity.hasRankInstanceId(_rankInstanceId))
                {
                    return activity;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过跨服实例id查找活动实例
     * @param _crossInstanceId
     * @return
     */
    public _AActivityBase lookupActivityByCrossInstanceId(long _crossInstanceId)
    {
        _lock();
        try
        {
            for (_AActivityBase activity : _m_alActivityList)
            {
                if (activity.getCrossInstanceId() == _crossInstanceId)
                {
                    return activity;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过活动组id查找活动实例
     * @param _groupId
     * @return
     */
    public _AActivityBase lookupActivityByGroupId(long _groupId)
    {
        _lock();
        try
        {
            for (_AActivityBase activity : _m_alActivityList)
            {
                if (activity.getSchedule().getUsGroupId() == _groupId)
                {
                    return activity;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查活动冲突
     * 同一activityId下，只允许一个实例处于非安全状态（INITIALIZING/PLAYING/SETTLING/REWARDING/CLOSED/RESTORE均视为冲突）
     * @param _activityId
     * @return
     */
    public boolean isConflictExist(long _activityId)
    {
        _lock();
        try
        {
            for (_AActivityBase activity : _m_alActivityList)
            {
                if (activity.getActivityId() == _activityId
                        && (activity.getCurState().getStateType() != EActivityState.PLAN
                        && activity.getCurState().getStateType() != EActivityState.CAN_DISCARD
                        && activity.getCurState().getStateType() != EActivityState.LOAD_FROM_DB))
                {
                    return true;
                }
            }
            return false;
        } finally
        {
            _unlock();
        }
    }

    public static class ActivityStepRewardBaseInfo
    {
        public long activityInstanceId;
        public ActivityStepRewardInfo stepReward;
    }

    /**
     * 构造阶段奖励信息列表
     * @param _cid
     * @return
     */
    public List<Activity_StepRewardInfo> makeStepRewardProto(long _cid)
    {
        List<Activity_StepRewardInfo> protoList = new ArrayList<>();
        List<ActivityStepRewardBaseInfo> list = new ArrayList<>();
        _lock();
        try{
            for (_AActivityBase activity : _m_alActivityList)
            {
                activity.fillStepRewardInfo(list);
            }
        }finally{
            _unlock();
        }

        for (ActivityStepRewardBaseInfo baseInfo : list)
        {
            Activity_StepRewardInfo proto = new Activity_StepRewardInfo();
            proto.setActivityInstanceId(baseInfo.activityInstanceId);
            ActivityStepRewardInfo stepReward = baseInfo.stepReward;
            proto.setStepRewardId(stepReward.getStepRewardId());
            proto.setScore(stepReward.getStepRewardScoreObjScore(_cid));
            proto.getHadDrawStepList().addAll(stepReward.getHadDrawStepList(_cid));
            stepReward.makeEventTaskProto(_cid, proto.getEventTaskList());
            protoList.add(proto);
        }

        return protoList;
    }
}
