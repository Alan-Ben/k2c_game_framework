package NPUSServer.NPUSUserMgr.UserComp.ActivityFund;

import ALBasicServer.ALProcess.ALProcess;
import Common.ActivityEnum.EActivityState;
import Common.ActivityFundObj.ActivityFund_Info;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.ActivityFundErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.ActivityFund.RefActivityFund;
import NPGameRes.Refs.ActivityFund.RefActivityFundLevel;
import NPGameRes.Refs.ActivityFund.RefActivityFundTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_ACTIVITY_FUND_ADD;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerActivityFundBO;
import USDB.Bo.PlayerActivityFundTaskBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动基金组件
 * <p>
 * 主要功能：
 * 1. 管理玩家的活动基金数据
 * 2. 处理任务触发和计数
 * 3. 处理阶段奖励领取
 * 4. 分数和等级管理
 * 5. 协议推送
 * <p>
 * 设计特点：
 * - 支持多个活动基金实例
 * - 任务事件驱动
 * - 懒加载数据库插入
 * - 等级缓存优化
 * <p>
 * 线程安全：通过玩家锁保证线程安全
 */
public class ActivityFundComponent extends _ANPUserComponent implements _IHandlerHolder
{
    // 玩家的所有活动基金列表
    private List<ActivityFundInfo> _m_fundList;

    /**
     * 构造函数
     * @param _userData 玩家数据
     */
    public ActivityFundComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ACTIVITY_FUND);

        _m_fundList = new ArrayList<>();
    }

    /**
     * 组件初始化
     * <p>
     * 执行流程：
     * 1. 加载主表数据（player_activity_fund）
     * 2. 加载任务表数据（player_activity_fund_task）
     * 3. 初始化完成
     */
    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("activity_fund_init");

        // 步骤1: 加载主表数据
        process.addResDelegateProcess(
                action -> _initMainDataFromDB(action::dealAction),
                "init_main_data",
                () -> USLog.error(getUSServer(), "ActivityFundComponent._init - load main data failed, cid={}", getUserData().getCid()),
                false);

        // 步骤2: 加载任务表数据
        process.addResDelegateProcess(
                action -> _initTaskDataFromDB(action::dealAction),
                "init_task_data",
                () -> USLog.error(getUSServer(), "ActivityFundComponent._init - load task data failed, cid={}", getUserData().getCid()),
                false);

        // 开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "ActivityFundComponent._init - process stopped, cid={}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 从数据库加载主表数据
     * @param _handler 回调处理器
     */
    private void _initMainDataFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerActivityFundBO.class).findAll(
                "cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerActivityFundBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerActivityFundBO> _boList)
                    {
                        // 加载数据
                        for (PlayerActivityFundBO bo : _boList)
                        {
                            RefActivityFund ref = RefActivityFund.getMgr().get(bo.getFundId());
                            if (ref == null)
                            {
                                USLog.error(getUSServer(), "ActivityFundComponent._initMainDataFromDB - ref not found, skip record: cid={}, fundId={}",
                                        getUserData().getCid(), bo.getFundId());
                                continue;
                            }

                            // 创建基金信息
                            ActivityFundInfo fundInfo = new ActivityFundInfo(ActivityFundComponent.this, ref, bo);
                            _m_fundList.add(fundInfo);
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            USLog.error(getUSServer(), "ActivityFundComponent._initMainDataFromDB - db error: cid={}", getUserData().getCid());
                            _handler.onRunOver(false);
                            return;
                        }

                        // 没有数据，正常情况
                        _handler.onRunOver(true);
                    }
                });
    }

    /**
     * 从数据库加载任务表数据
     * @param _handler 回调处理器
     */
    private void _initTaskDataFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerActivityFundTaskBO.class).findAll(
                "cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerActivityFundTaskBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerActivityFundTaskBO> _boList)
                    {
                        // 按基金记录ID分组
                        for (PlayerActivityFundTaskBO taskBo : _boList)
                        {
                            ActivityFundInfo fundInfo = lookupActivityFundInfoByDbId(taskBo.getFundRecordId());
                            if (fundInfo == null)
                            {
                                USLog.error(getUSServer(), "ActivityFundComponent._initTaskDataFromDB - fund not found, skip record: cid={}, fundRecordId={}",
                                        getUserData().getCid(), taskBo.getFundRecordId());
                                continue;
                            }

                            RefActivityFundTask taskRef = RefActivityFundTask.getMgr().get(taskBo.getTaskId());
                            if (taskRef == null)
                            {
                                USLog.error(getUSServer(), "ActivityFundComponent._initTaskDataFromDB - task ref not found, skip record: cid={}, taskId={}",
                                        getUserData().getCid(), taskBo.getTaskId());
                                continue;
                            }

                            // 添加任务数据到对应的基金
                            fundInfo.initTaskDataFromDB(taskRef, taskBo);
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            USLog.error(getUSServer(), "ActivityFundComponent._initTaskDataFromDB - db error: cid={}", getUserData().getCid());
                            _handler.onRunOver(false);
                            return;
                        }

                        // 没有数据，正常情况
                        _handler.onRunOver(true);
                    }
                });
    }

    /**
     * 声明依赖组件
     * @return 依赖组件列表
     */
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null; // 暂无依赖
    }

    /**
     * 初始化完成回调
     * <p>
     * 注册事件监听
     */
    @Override
    public void onInited()
    {
        // 注册所有基金的事件监听器
        for (ActivityFundInfo fundInfo : _m_fundList)
        {
            fundInfo.initTaskAndRegEvent();
        }

        // 便利常驻基金，初始化任务和注册事件
        List<RefActivityFund> refList = RefActivityFund.getMgr().getList();
        for (RefActivityFund refFund : refList)
        {
            if (refFund.activity_id != 0)
                continue;

            // 永久基金活动开始时间为0
            addActivityFund(refFund, 0, 0);
        }

        // 注册活动状态变更监听器
        getUSServer().getCommActivityMgr().activityStateChg.addHandler(this, new HandlerTwo<_AActivityBase, EActivityState>()
        {
            @Override
            public void handle(_AActivityBase _activity, EActivityState _state)
            {
                if (_AActivityBase.RUNNING_STATE.contains(_state))
                {
                    RefActivityFund ref = RefActivityFund.getMgr().getRefByActivityId(_activity.getActivityId());
                    if (ref == null)
                        return;

                    // 活动基金传入活动开始时间
                    addActivityFund(ref, _activity.getInstanceId(), _activity.getStartTimeMs());
                } else if (_AActivityBase.CLOSE_STATE.contains(_state))
                {
                    RefActivityFund ref = RefActivityFund.getMgr().getRefByActivityId(_activity.getActivityId());
                    if (ref == null)
                        return;

                    removeActivityFund(ref.activity_fund_id);
                }
            }
        });

        //初始化检查活动基金
        _initCheckActivityFund();
    }

    /**
     * 初始化检查活动基金
     */
    private void _initCheckActivityFund()
    {
        getUserData().lockUser();
        try
        {
            // 检查已存在的活动实例
            List<_AActivityBase> runningActivities = getUSServer().getCommActivityMgr().getAllActivity();
            for (_AActivityBase activity : runningActivities)
            {
                if (!activity.isRunning())
                    continue;

                RefActivityFund ref = RefActivityFund.getMgr().getRefByActivityId(activity.getActivityId());
                if (ref != null)
                {
                    // 活动基金传入活动开始时间
                    addActivityFund(ref, activity.getInstanceId(), activity.getStartTimeMs());
                }
            }

            // 检查玩家是否有已经关闭的活动基金需要移除
            List<ActivityFundInfo> fundsToRemove = new ArrayList<>();
            for (ActivityFundInfo fundInfo : _m_fundList)
            {
                if (fundInfo.getActivityInstanceId() != 0)
                {
                    _AActivityBase activity = getUSServer().getCommActivityMgr()
                            .lookupActivity(fundInfo.getActivityInstanceId());
                    if (activity == null || activity.isClosing())
                    {
                        fundsToRemove.add(fundInfo);
                    }
                }
            }
            for (ActivityFundInfo fundInfo : fundsToRemove)
            {
                removeActivityFund(fundInfo.getFundId());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 资源清理
     * <p>
     * 清理事件监听器
     */
    @Override
    public void dispose()
    {
        // 清理所有基金的事件监听器
        for (ActivityFundInfo fundInfo : _m_fundList)
        {
            fundInfo.unregAllEvent();
        }

        // 清理组件事件监听器
        getUSServer().getCommActivityMgr().activityStateChg.clear(this);
    }

    /**
     * 根据基金ID获取活动基金信息
     * @param _fundId 基金ID
     * @return 基金信息
     */
    public ActivityFundInfo lookupActivityFundInfoByFundId(long _fundId)
    {
        getUserData().lockUser();
        try
        {
            for (ActivityFundInfo fundInfo : _m_fundList)
            {
                if (fundInfo.getFundId() == _fundId)
                {
                    return fundInfo;
                }
            }
            return null;

        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 根据数据库ID获取活动基金信息
     * @param _dbId 数据库记录ID
     * @return 基金信息
     */
    public ActivityFundInfo lookupActivityFundInfoByDbId(long _dbId)
    {
        getUserData().lockUser();
        try
        {
            for (ActivityFundInfo fundInfo : _m_fundList)
            {
                if (fundInfo.getDbId() == _dbId)
                {
                    return fundInfo;
                }
            }
            return null;

        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 一键领取所有可领取的阶段奖励
     * @param _fundId  基金ID
     * @param _context 操作上下文
     * @return 领取结果
     */
    public Result drawAllAvailableRewards(long _fundId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 获取基金信息
            ActivityFundInfo fundInfo = lookupActivityFundInfoByFundId(_fundId);
            if (fundInfo == null)
                return ActivityFundErr.FUND_NOT_FOUND;

            // 领取奖励
            return fundInfo.drawAllAvailableRewards(_context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加活动基金
     * @param _fundRef 基金配置
     * @param _activityInstanceId 活动实例ID（常驻基金为0）
     * @param _activityStartTimeMs 活动开始时间（毫秒，永久基金为0）
     * @return 新创建的基金信息，如果配置不存在则返回null
     */
    public ActivityFundInfo addActivityFund(RefActivityFund _fundRef, long _activityInstanceId, long _activityStartTimeMs)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否已存在
            ActivityFundInfo existingFund = lookupActivityFundInfoByFundId(_fundRef.activity_fund_id);
            if (existingFund != null)
                return existingFund;

            // 获取配置
            RefActivityFund ref = RefActivityFund.getMgr().get(_fundRef.activity_fund_id);
            if (ref == null)
            {
                USLog.error(getUSServer(), "ActivityFundComponent.addActivityFund - ref not found: fundId={}", _fundRef.activity_fund_id);
                return null;
            }

            // 创建新基金
            ActivityFundInfo fundInfo = new ActivityFundInfo(this, ref, _activityInstanceId, _activityStartTimeMs);
            _m_fundList.add(fundInfo);

            // 初始化任务和注册事件
            fundInfo.initTaskAndRegEvent();

            // 推送新增基金协议
            getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_067_OnActivityFundAdd(fundInfo.toProto()));

            // 触发新增基金事件
            if (ref.activity_id != 0)
                getUserData().onLogicEvent(
                        new Event_P_ACTIVITY_FUND_ADD(NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_FUND_ADD)));

            // 遍历清除活动基金相关的所有礼包的购买记录
            clearAllLevelGiftPackRecords(ref.activity_fund_id);

            return fundInfo;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 移除活动基金
     * <p>
     * 执行流程：
     * 1. 查找活动基金信息
     * 2. 收集并通过邮件补发玩家未领取的奖励
     * 3. 清除所有等级的凭证道具
     * 4. 从列表中移除
     * 5. 销毁基金数据（数据库删除、事件取消等）
     * 6. 推送移除基金协议
     * @param _fundId 基金ID
     */
    public void removeActivityFund(long _fundId)
    {
        getUserData().lockUser();
        try
        {
            ActivityFundInfo fundInfo = lookupActivityFundInfoByFundId(_fundId);
            if (fundInfo == null)
                return;

            // 通过邮件补发未领取的奖励
            fundInfo.sendUnclaimedRewardsEmail();

            _m_fundList.remove(fundInfo);
            fundInfo.discard();

            // 推送移除基金协议
            getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_068_OnActivityFundRemove(_fundId));

            // 清除所有等级的凭证道具
            removeAllLevelDistinguishItems(fundInfo);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除活动基金所有等级的凭证道具
     * <p>
     * 遍历该活动的所有等级配置，移除玩家拥有的每个等级的凭证道具
     * @param _fundInfo 活动基金信息
     */
    private void removeAllLevelDistinguishItems(ActivityFundInfo _fundInfo)
    {
        getUserData().lockUser();
        try
        {
            if (_fundInfo == null)
                return;

            // 获取该基金的所有等级配置
            List<RefActivityFundLevel> levelList = RefActivityFundLevel.getMgr()
                    .getLevelListByFundId(_fundInfo.getFundId());
            if (levelList == null || levelList.isEmpty())
                return;

            // 创建上下文（使用活动基金领取奖励事件，因为移除时也涉及道具操作）
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_FUND_DRAW_REWARD);

            // 遍历所有等级，移除凭证道具
            for (RefActivityFundLevel levelRef : levelList)
            {
                if (levelRef.distinguish_item == null)
                    continue;

                // 检查玩家是否拥有该凭证道具
                long itemCount = getUserData().getItemCount(levelRef.distinguish_item);
                if (itemCount > 0)
                {
                    // 移除凭证道具
                    getUserData().spendItem(levelRef.distinguish_item, itemCount, context);
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除活动基金所有等级关联的礼包购买记录
     * <p>
     * 遍历该活动基金的所有等级配置，清除每个等级对应的付费礼包的购买记录
     * 用于活动基金开启时重置玩家的礼包购买状态
     * @param _fundId 基金ID
     */
    private void clearAllLevelGiftPackRecords(long _fundId)
    {
        getUserData().lockUser();
        try
        {
            // 获取该基金的所有等级配置
            List<RefActivityFundLevel> levelList = RefActivityFundLevel.getMgr()
                    .getLevelListByFundId(_fundId);
            if (levelList == null || levelList.isEmpty())
                return;

            // 创建上下文
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_FUND_ADD);

            // 遍历所有等级，清除对应的礼包购买记录
            for (RefActivityFundLevel levelRef : levelList)
            {
                // 如果该等级没有配置礼包ID，跳过
                if (levelRef.gift_pack_id <= 0)
                    continue;

                // 清除该礼包的购买记录
                getUserData().getOrderComponent().getMgr().clearPurchaseRecord(levelRef.gift_pack_id, context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 转换为协议对象列表
     * @return 协议对象列表
     */
    public List<ActivityFund_Info> makeProtoList()
    {
        getUserData().lockUser();
        try
        {
            getUserData().lockUser();
            try
            {
                List<ActivityFund_Info> result = new ArrayList<>();
                for (ActivityFundInfo fundInfo : _m_fundList)
                {
                    result.add(fundInfo.toProto());
                }
                return result;
            } finally
            {
                getUserData().unlockUser();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
