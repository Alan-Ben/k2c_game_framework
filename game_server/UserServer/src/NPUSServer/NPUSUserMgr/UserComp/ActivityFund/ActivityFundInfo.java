package NPUSServer.NPUSUserMgr.UserComp.ActivityFund;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityFundObj.ActivityFund_Info;
import Common.ActivityFundObj.ActivityFund_TaskInfo;
import Common.MailObj.Mail_Data;
import EventSystem.NPHandlerEntry;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ActivityFundErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.ActivityFund.RefActivityFund;
import NPGameRes.Refs.ActivityFund.RefActivityFundLevel;
import NPGameRes.Refs.ActivityFund.RefActivityFundStep;
import NPGameRes.Refs.ActivityFund.RefActivityFundTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerActivityFundBO;
import USDB.Bo.PlayerActivityFundTaskBO;
import USLOGDB.Bo.LogActivityFundDrawBO;
import USLOGDB.Bo.LogActivityFundScoreBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动基金信息
 * <p>
 * 主要功能：
 * 1. 管理单个活动基金的数据和业务逻辑
 * 2. 分数管理（任务分数 + 公式分数 + 激活经验值）
 * 3. 阶段管理（根据总分数动态计算当前阶段）
 * 4. 一键领取所有可领取的阶段奖励
 * 5. 任务触发和计数管理
 * 6. 懒加载数据库插入
 * <p>
 * 设计特点：
 * - 总分 = taskScore(任务分数) + formulaScore(公式分数) + activateExp(激活经验值)
 * - 激活经验值动态计算（遍历等级检查凭证道具）
 * - 当前阶段动态计算（不缓存，每次根据总分实时计算）
 * - 已领取阶段使用单个int记录最大阶段号
 * - 公式分数独立存储，通过事件刷新
 * - 一键领取机制：自动领取所有满足条件的阶段奖励
 * <p>
 * 线程安全：通过玩家锁保证线程安全
 */
public class ActivityFundInfo implements _IHandlerHolder
{
    // 所属组件
    private ActivityFundComponent _m_comp;

    // 数据库ID，0表示未插入数据库
    private long _m_dbId;

    // 活动实例ID（常驻基金为0）
    private long _m_activityInstanceId;

    // 活动开始时间（毫秒，永久基金为0）
    private long _m_activityStartTimeMs;

    // 上次刷新轮次号
    private long _m_lastRefreshRound;

    // GM临时任务刷新时间（毫秒时间戳，0表示不使用）
    private long _m_gmTaskRefreshTimeMs;

    // 公式分数
    private long _m_formulaScore;

    // 任务分数
    private long _m_taskScore;

    // 已领取免费档
    private int _m_hadDrawFreeStep;

    // 已领取付费档
    private int _m_hadDrawPayStep;

    // 任务信息列表
    private List<ActivityFundTaskInfo> _m_taskList;

    // 配置缓存
    private RefActivityFund _m_refFund;

    // 事件监听器列表
    private List<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    /**
     * 构造函数 - 用于新建基金
     * @param _comp 所属组件
     * @param _ref 配置引用
     * @param _activityInstanceId 活动实例ID（常驻基金为0）
     * @param _activityStartTimeMs 活动开始时间（毫秒，永久基金为0）
     */
    public ActivityFundInfo(ActivityFundComponent _comp, RefActivityFund _ref, long _activityInstanceId, long _activityStartTimeMs)
    {
        _m_comp = _comp;
        _m_activityInstanceId = _activityInstanceId;
        _m_activityStartTimeMs = _activityStartTimeMs;
        _m_lastRefreshRound = 0;
        _m_taskList = new ArrayList<>();
        _m_evtEntryList = new ArrayList<>();

        // 配置对象
        _m_refFund = _ref;
    }

    /**
     * 构造函数 - 用于从数据库加载
     * @param _component 所属组件
     * @param _ref       配置引用
     * @param _bo        数据库BO对象
     */
    public ActivityFundInfo(ActivityFundComponent _component, RefActivityFund _ref, PlayerActivityFundBO _bo)
    {
        this(_component, _ref, _bo.getActivityInstanceId(), _bo.getActivityStartTimeMs());

        _m_dbId = _bo.getId();
        _m_taskScore = _bo.getTaskScore();
        _m_formulaScore = _bo.getFormulaScore();
        _m_hadDrawFreeStep = _bo.getDrawnFreeSteps();
        _m_hadDrawPayStep = _bo.getDrawnPaidSteps();
        _m_lastRefreshRound = _bo.getLastRefreshRound();
    }

    // Getters
    public ActivityFundComponent getComponent()
    {
        return _m_comp;
    }

    public long getFundId()
    {
        return _m_refFund.activity_fund_id;
    }

    public long getActivityInstanceId()
    {
        return _m_activityInstanceId;
    }

    public long getActivityId()
    {
        return _m_refFund.activity_id;
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 获取当前阶段（动态计算）
     * <p>
     * 根据总分数实时计算当前可达阶段，不缓存结果
     * @return 当前阶段号
     */
    public int getCurrentStep()
    {
        return RefActivityFundStep.getMgr().calculateCurrentStep(getFundId(), getTotalScore());
    }

    public int getHadDrawFreeStep()
    {
        return _m_hadDrawFreeStep;
    }

    public int getHadDrawPayStep()
    {
        return _m_hadDrawPayStep;
    }

    public BM getBM()
    {
        return _m_comp.getUSServer().getBM();
    }

    public NPUserServer getUSServer()
    {
        return _m_comp.getUSServer();
    }

    public RefActivityFund getRef()
    {
        return _m_refFund;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    public long getNextTaskRefreshTimeMs()
    {
        getUserData().lockUser();
        try
        {
            // 优先返回 GM 设置的刷新时间
            if (_m_gmTaskRefreshTimeMs > 0)
                return _m_gmTaskRefreshTimeMs;

            return _m_activityStartTimeMs + (_m_lastRefreshRound + 1) * _m_refFund.task_refresh_time * 1000L;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 初始化任务数据从数据库BO
     * @param _taskRef
     * @param _taskBo
     */
    public void initTaskDataFromDB(RefActivityFundTask _taskRef, PlayerActivityFundTaskBO _taskBo)
    {
        _m_taskList.add(new ActivityFundTaskInfo(this, _taskRef, _taskBo));
    }

    /**
     * 初始化所有任务
     */
    private void initAllTasks()
    {
        getUserData().lockUser();
        try
        {
            if (_m_refFund == null)
                return;

            if (_m_refFund.task_group_id == 0)
                return;

            // 通过任务组ID查找任务列表
            List<RefActivityFundTask> taskRefs = RefActivityFundTask.getMgr().getTaskListByGroupId(_m_refFund.task_group_id);
            if (taskRefs == null)
                return;

            for (RefActivityFundTask taskRef : taskRefs)
            {
                ActivityFundTaskInfo taskInfo = new ActivityFundTaskInfo(this, taskRef);
                _m_taskList.add(taskInfo);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 初始化任务并注册事件监听器
     */
    public void initTaskAndRegEvent()
    {
        getUserData().lockUser();
        try
        {
            // 初始化所有任务
            initAllTasks();

            // 注册公式分数的事件监听器
            regEvtEntry();

            // 计算初始公式分数
            recalFormulaScore(_m_comp.getUserData().getPlayerInitContext());

            // 注册任务的事件监听器
            for (ActivityFundTaskInfo taskInfo : _m_taskList)
            {
                taskInfo.regEvtEntry();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 取消所有事件监听器
     */
    public void unregAllEvent()
    {
        getUserData().lockUser();
        try
        {
            // 取消公式分数的事件监听器
            unregEvtEntry();

            // 取消任务的事件监听器
            for (ActivityFundTaskInfo taskInfo : _m_taskList)
            {
                taskInfo.unregEvtEntry();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取总分数（任务分数 + 公式分数 + 激活经验值）
     * <p>
     * 激活经验值通过遍历等级配置动态计算：
     * 检查玩家是否拥有每个等级的凭证道具，如果拥有则累加该等级的activate_exp_count
     *
     * @return 总分数
     */
    public long getTotalScore()
    {
        getUserData().lockUser();
        try
        {
            long activateExp = 0;

            // 获取所有等级配置
            List<RefActivityFundLevel> levelList = RefActivityFundLevel.getMgr().getLevelListByFundId(getFundId());
            if (levelList != null)
            {
                // 遍历所有等级，累加已激活等级的经验值
                for (RefActivityFundLevel levelRef : levelList)
                {
                    // 检查是否拥有该等级的凭证道具
                    if (hasDistinguishItem(levelRef))
                    {
                        activateExp += levelRef.activate_exp_count;
                    }
                }
            }

            return _m_taskScore + _m_formulaScore + activateExp;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 计算公式分数
     * @return 公式计算的分数
     */
    private long calculateFormulaScore()
    {
        getUserData().lockUser();
        try
        {
            if (_m_refFund == null)
                return 0;

            if (_m_refFund.process_cur_count == null)
                return 0;

            return NPPlayerVariableDeal.getInstance().CalculateVariableResult(
                    _m_comp.getUserData(), _m_refFund.process_cur_count, null);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加任务分数
     * <p>
     * 执行流程：
     * 1. 增加任务分数
     * 2. 更新当前阶段
     * 3. 保存到数据库
     * 4. 推送分数变化协议
     * @param _score   增加的分数
     * @param _context 操作上下文
     */
    public void addTaskScore(long _score, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 记录变动前分数
            long scoreBefore = _m_taskScore;

            // 增加任务分数
            _m_taskScore += _score;

            // 记录分数变动日志(类型2=任务分数)
            recordScoreLog(2, scoreBefore, _m_taskScore, _context);

            // 保存到数据库
            if (!tryCreateInDB())
            {
                // 已存在记录，使用增量更新
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("task_score", _m_taskScore);
                getBM().getBM(PlayerActivityFundBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送分数变化协议
            _m_comp.getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_064_OnActivityFundScoreChg(
                            getFundId(), _m_formulaScore, _m_taskScore));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否有指定等级的凭证道具
     *
     * @param _levelRef 等级配置
     * @return true=有凭证，false=无凭证
     */
    private boolean hasDistinguishItem(RefActivityFundLevel _levelRef)
    {
        getUserData().lockUser();
        try
        {
            if (_levelRef == null)
                return false;

            if (_levelRef.distinguish_item == null)
                return false;

            return _m_comp.getUserData().hasItem(_levelRef.distinguish_item, 1);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 一键领取所有可领取的阶段奖励（玩家主动领取）
     * <p>
     * 使用单等级模式处理奖励，一次只处理首个未完成等级
     * 发放奖励后推送协议并记录日志
     *
     * @param _context 操作上下文
     * @return 领取结果
     */
    public Result drawAllAvailableRewards(NPPlayerContext _context)
    {
getUserData().lockUser();
try
{
            // 记录领取前的状态
            int hadDrawFreeStepBefore = _m_hadDrawFreeStep;
            int hadDrawPayStepBefore = _m_hadDrawPayStep;

            // 调用公共方法获取奖励列表
            ResultOne<List<NPCommonCostItem>> result = processAvailableRewards(RewardCollectMode.SINGLE_LEVEL_ONLY);

            // 失败则返回错误码
            if (!result.isSucc())
                return Result.failed(result.getCode());

            // 记录领取后的状态
            int hadDrawFreeStepAfter = _m_hadDrawFreeStep;
            int hadDrawPayStepAfter = _m_hadDrawPayStep;

            // 成功则发放奖励
            _m_comp.getUserData().gainItemList(result.getData(), _context);

            // 推送领取奖励变化协议
            _m_comp.getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_066_OnActivityFundDrawRewardChg(
                            getFundId(), _m_hadDrawFreeStep, _m_hadDrawPayStep));

            // 记录数据日志
            recordDrawLog(hadDrawFreeStepBefore, hadDrawPayStepBefore,
                    hadDrawFreeStepAfter, hadDrawPayStepAfter, _context);

            return Result.SUCC;
} finally
{
    getUserData().unlockUser();
}
    }

    /**
     * 尝试在数据库创建记录（懒加载）
     * @return true=首次创建，false=已存在
     */
    public boolean tryCreateInDB()
    {
        getUserData().lockUser();
        try
        {
            if (_m_dbId != 0)
                return false; // 已插入

            // 创建BO对象并插入数据库
            PlayerActivityFundBO bo = new PlayerActivityFundBO();
            bo.setCid(getBM(), _m_comp.getUserData().getCid());
            bo.setFundId(getBM(), getFundId());
            bo.setActivityInstanceId(getBM(), _m_activityInstanceId);
            bo.setFormulaScore(getBM(), _m_formulaScore);
            bo.setTaskScore(getBM(), _m_taskScore);
            bo.setDrawnFreeSteps(getBM(), _m_hadDrawFreeStep);
            bo.setDrawnPaidSteps(getBM(), _m_hadDrawPayStep);
            bo.setActivityStartTimeMs(getBM(), _m_activityStartTimeMs);
            bo.setLastRefreshRound(getBM(), _m_lastRefreshRound);
            bo.insert(getBM());

            _m_dbId = bo.getId();
            return true; // 首次插入返回true
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 注册事件监听
     * <p>
     * 监听公式分数刷新事件
     */
    protected void regEvtEntry()
    {
        getUserData().lockUser();
        try
        {
            if (_m_refFund == null)
                return;

            if (_m_refFund.refresh_process_cur_count_type == null || _m_refFund.refresh_process_cur_count_type.isEmpty())
                return;

            EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(
                    _m_refFund.refresh_process_cur_count_type.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("ActivityFundInfo.regEvtEntry - event not found: activityId={}, event={}",
                        getActivityId(), _m_refFund.refresh_process_cur_count_type, new Exception());
                return;
            }

            // 注册事件监听
            NPHandlerEntry<NPUSUserData> evtEntry = _m_comp.getUserData().getEventHandlerMgr().regHandler(
                    eventMeta.getEventId(), this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                    {
                        @Override
                        public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                        {
                            NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                            recalFormulaScore(context);
                        }
                    });

            _m_evtEntryList.add(evtEntry);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 取消事件监听
     */
    public void unregEvtEntry()
    {
        getUserData().lockUser();
        try
        {
            if (_m_evtEntryList.isEmpty()) return;

            for (NPHandlerEntry<NPUSUserData> entry : _m_evtEntryList)
            {
                _m_comp.getUserData().getEventHandlerMgr().unregHandler(entry);
            }
            _m_evtEntryList.clear();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 处理逻辑事件
     * <p>
     * 当公式分数刷新事件触发时，重新计算公式分数并更新当前阶段
     * @param _context 操作上下文
     */
    protected void recalFormulaScore(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 记录变动前分数
            long scoreBefore = _m_formulaScore;

            // 重新计算公式分数
            _m_formulaScore = calculateFormulaScore();

            // 记录分数变动日志(类型1=公式分数)
            recordScoreLog(1, scoreBefore, _m_formulaScore, _context);

            // 保存到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("formula_score", _m_formulaScore);
                getBM().getBM(PlayerActivityFundBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送分数变化协议
            _m_comp.getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_064_OnActivityFundScoreChg(
                            getFundId(), _m_formulaScore, _m_taskScore));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 收集未领取的奖励并通过邮件发送给玩家（活动结束补偿）
     * <p>
     * 使用全等级模式处理奖励，一次性收集所有等级的未领取奖励
     * 通过邮件发放，适用于玩家离线或活动结束场景
     */
    public void sendUnclaimedRewardsEmail()
    {
        getUserData().lockUser();
        try
        {
            // 记录领取前的状态
            int hadDrawFreeStepBefore = _m_hadDrawFreeStep;
            int hadDrawPayStepBefore = _m_hadDrawPayStep;

            // 调用公共方法获取奖励列表
            ResultOne<List<NPCommonCostItem>> result = processAvailableRewards(RewardCollectMode.ALL_LEVELS);

            // 失败则记录日志并返回
            if (!result.isSucc())
            {
                CommLog.error("ActivityFundInfo sendUnclaimedRewardsEmail - failed to process unclaimed rewards: cid={}, fundId={}, errCode={}",
                        _m_comp.getUserData().getCid(), getFundId(), result.getCode());
                return;
            }

            // 获取奖励列表
            List<NPCommonCostItem> rewardList = result.getData();
            if (rewardList.isEmpty())
                return;

            // 构造并发送邮件
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(_m_refFund.mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(rewardList));

            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE);

            MailSystem.addMail(_m_comp.getUSServer(), _m_comp.getUserData().getCid(), mailData, context);

            // 记录领取后的状态
            int hadDrawFreeStepAfter = _m_hadDrawFreeStep;
            int hadDrawPayStepAfter = _m_hadDrawPayStep;

            // 记录数据日志
            recordDrawLog(hadDrawFreeStepBefore, hadDrawPayStepBefore,
                    hadDrawFreeStepAfter, hadDrawPayStepAfter, context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 销毁对象
     */
    public void discard()
    {
        getUserData().lockUser();
        try
        {
            if (_m_dbId != 0)
                getBM().getBM(PlayerActivityFundBO.class).delAll("id", _m_dbId);

            for (ActivityFundTaskInfo taskInfo : _m_taskList)
            {
                taskInfo.discard();
            }

            // 取消所有事件监听
            unregAllEvent();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 转换为协议对象
     * @return 协议对象
     */
    public ActivityFund_Info toProto()
    {
        getUserData().lockUser();
        try
        {
            ActivityFund_Info obj = new ActivityFund_Info();
            obj.setFundId(getFundId());
            obj.setActivityInstanceId(_m_activityInstanceId);
            obj.setFormulaScore(_m_formulaScore);
            obj.setTaskScore(_m_taskScore);
            obj.setHadDrawFreeStep(_m_hadDrawFreeStep);
            obj.setHadDrawPayStep(_m_hadDrawPayStep);
            return obj;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 生成任务协议列表
     * @return 任务协议列表
     */
    public List<ActivityFund_TaskInfo> makeTaskProtoList()
    {
        getUserData().lockUser();
        try
        {
            List<ActivityFund_TaskInfo> result = new ArrayList<>();
            for (ActivityFundTaskInfo taskInfo : _m_taskList)
            {
                result.add(taskInfo.toProto());
            }
            return result;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 计算当前任务轮次号
     * <p>
     * 以活动开启时间为基准，每 task_refresh_time 秒为一轮
     * 轮次号从1开始递增
     *
     * @return 当前轮次号，如果不支持轮次刷新则返回0
     */
    public long calculateCurrentRefreshRound()
    {
        getUserData().lockUser();
        try
        {
            // 检查配置是否支持任务刷新
            if (_m_refFund == null)
                return 0;

            // 刷新时间配置为0或负数表示不刷新
            if (_m_refFund.task_refresh_time <= 0)
                return 0;

            // 永久基金不刷新
            if (_m_activityStartTimeMs == 0)
                return 0;

            // 当前时间
            long nowMs = CommonFunc.getNowTimeMS();

            // 计算距离活动开启的时间差（毫秒）
            long elapsedMs = nowMs - _m_activityStartTimeMs;
            if (elapsedMs < 0)
                return 0;

            // 计算当前轮次号（从1开始）
            // 轮次号 = (经过的时间 / 刷新间隔) + 1
            long refreshIntervalMs = _m_refFund.task_refresh_time * 1000L;
            return (elapsedMs / refreshIntervalMs) + 1;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查并重置过期的任务数据
     * <p>
     * 执行流程：
     * 1. 优先检查GM临时刷新时间
     * 2. 检查是否配置了任务刷新时间
     * 3. 计算当前轮次号
     * 4. 如果轮次号发生变化，批量删除所有任务数据并重置内存对象
     * 5. 更新并保存轮次号
     */
    public void checkAndResetExpiredTasks()
    {
        getUserData().lockUser();
        try
        {
            // 优先检查GM临时刷新时间
            if (_m_gmTaskRefreshTimeMs > 0)
            {
                long nowMs = CommonFunc.getNowTimeMS();
                if (nowMs >= _m_gmTaskRefreshTimeMs)
                {
                    // 达到GM设置的刷新时间，清空任务数据
                    resetAllTasksData();
                    // 清空GM刷新时间标记
                    _m_gmTaskRefreshTimeMs = 0;
                    return;
                }
            }

            // 检查配置是否支持任务刷新
            if (_m_refFund == null)
                return;

            // 刷新时间配置为0或负数表示不刷新
            if (_m_refFund.task_refresh_time <= 0)
                return;

            // 永久基金不刷新
            if (_m_activityStartTimeMs == 0)
                return;

            // 计算当前轮次号
            long currentRound = calculateCurrentRefreshRound();
            if (currentRound == 0)
                return;

            // 如果轮次号未发生变化，无需重置
            if (_m_lastRefreshRound >= currentRound)
                return;

            // 重置所有任务数据
            resetAllTasksData();

            // 更新轮次号
            _m_lastRefreshRound = currentRound;

            // 保存轮次号到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("last_refresh_round", _m_lastRefreshRound);
                getBM().getBM(PlayerActivityFundBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 重置所有任务数据
     * <p>
     * 执行流程：
     * 1. 收集所有需要删除的任务数据库ID
     * 2. 重置任务到初始状态
     * 3. 批量删除数据库记录
     */
    private void resetAllTasksData()
    {
        getUserData().lockUser();
        try
        {
            // 收集所有需要删除的任务数据库ID
            List<Long> dbIds = new ArrayList<>();
            for (ActivityFundTaskInfo taskInfo : _m_taskList)
            {
                if (taskInfo.getDbId() != 0)
                {
                    dbIds.add(taskInfo.getDbId());
                }
                // 重置任务到初始状态
                taskInfo.resetToInitState();
            }

            // 批量删除数据库记录
            if (!dbIds.isEmpty())
            {
                getBM().getBM(PlayerActivityFundTaskBO.class).delAllInList("id", dbIds);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    // ========== GM命令相关方法 ==========

    /**
     * 设置免费档已领取阶段（GM命令使用）
     * <p>
     * 执行流程：
     * 1. 更新免费档已领取阶段值
     * 2. 保存到数据库
     * 3. 推送协议通知客户端
     * @param _step 阶段号
     */
    public void setHadDrawFreeStep(int _step)
    {
        getUserData().lockUser();
        try
        {
            _m_hadDrawFreeStep = _step;

            // 保存到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("drawn_free_steps", _m_hadDrawFreeStep);
                getBM().getBM(PlayerActivityFundBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送领取奖励变化协议
            _m_comp.getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_066_OnActivityFundDrawRewardChg(
                            getFundId(), _m_hadDrawFreeStep, _m_hadDrawPayStep));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置付费档已领取阶段（GM命令使用）
     * <p>
     * 执行流程：
     * 1. 更新付费档已领取阶段值
     * 2. 保存到数据库
     * 3. 推送协议通知客户端
     * @param _step 阶段号
     */
    public void setHadDrawPayStep(int _step)
    {
        getUserData().lockUser();
        try
        {
            _m_hadDrawPayStep = _step;

            // 保存到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("drawn_paid_steps", _m_hadDrawPayStep);
                getBM().getBM(PlayerActivityFundBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送领取奖励变化协议
            _m_comp.getUserData().sendMsgToGC(
                    US2GCWriter_017_ActivityOp.make_066_OnActivityFundDrawRewardChg(
                            getFundId(), _m_hadDrawFreeStep, _m_hadDrawPayStep));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置任务计数（GM命令使用）
     * <p>
     * 执行流程：
     * 1. 查找指定任务
     * 2. 设置任务计数（使用覆盖模式）
     * @param _taskId  任务ID
     * @param _count   计数值
     * @param _context 操作上下文
     */
    public void setTaskCount(long _taskId, long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            ActivityFundTaskInfo taskInfo = lookupTaskInfo(_taskId);
            if (taskInfo == null)
                return;

            taskInfo.setCount(_count, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加任务计数（GM命令使用）
     * <p>
     * 执行流程：
     * 1. 查找指定任务
     * 2. 增加任务计数
     * @param _taskId  任务ID
     * @param _count   增加的计数
     * @param _context 操作上下文
     */
    public void addTaskCount(long _taskId, long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            ActivityFundTaskInfo taskInfo = lookupTaskInfo(_taskId);
            if (taskInfo == null)
                return;

            taskInfo.addCount(_count, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置GM临时任务刷新时间（GM命令使用）
     * <p>
     * 设置后，当当前时间大于等于设置的时间时，会自动清空所有任务数据并重置刷新时间标记
     * 不需要保存到数据库，仅在内存中生效
     *
     * @param _refreshTimeMs GM刷新时间（毫秒时间戳，0表示取消）
     */
    public void setGmTaskRefreshTimeMs(long _refreshTimeMs)
    {
        getUserData().lockUser();
        try
        {
            _m_gmTaskRefreshTimeMs = _refreshTimeMs;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找任务信息
     * @param _taskId 任务ID
     * @return 任务信息，如果不存在则返回null
     */
    private ActivityFundTaskInfo lookupTaskInfo(long _taskId)
    {
       getUserData().lockUser();
       try
       {
            for (ActivityFundTaskInfo taskInfo : _m_taskList)
            {
                if (taskInfo.getTaskId() == _taskId)
                {
                    return taskInfo;
                }
            }
            return null;
       } finally
       {
           getUserData().unlockUser();
       }
    }

    /**
     * 处理可领取的奖励（共有逻辑提取）
     * <p>
     * 执行流程：
     * 1. 计算可领取的阶段区间
     * 2. 获取所有等级配置
     * 3. 按等级遍历，收集奖励
     * 4. 更新数据库
     * 5. 返回奖励列表
     * <p>
     * @param mode 奖励收集模式（单等级/全部等级）
     * @return 成功返回奖励列表，失败返回错误码
     */
    private ResultOne<List<NPCommonCostItem>> processAvailableRewards(RewardCollectMode mode)
    {
        getUserData().lockUser();
        try
        {
            // 1. 计算可领取的阶段区间
            int startStep = Math.min(_m_hadDrawFreeStep, _m_hadDrawPayStep) + 1;
            int endStep = getCurrentStep();

            if (endStep < startStep)
                return ResultOne.failed(ActivityFundErr.ALREADY_DRAWN);

            // 2. 获取所有等级配置
            List<RefActivityFundLevel> levelList =
                    RefActivityFundLevel.getMgr().getLevelListByFundId(getFundId());
            if (levelList == null || levelList.isEmpty())
                return ResultOne.failed(ActivityFundErr.LEVEL_NOT_FOUND);

            // 3. 初始化收集容器
            List<NPCommonCostItem> rewardList = new ArrayList<>();
            int newMaxFreeStep = _m_hadDrawFreeStep;
            int newMaxPayStep = _m_hadDrawPayStep;
            boolean hasProgress = false;

            // 4. 遍历所有等级，收集奖励
            int prevLevelLastStep = 0;
            for (RefActivityFundLevel levelRef : levelList)
            {
                // 检查当前等级是否已经完成（仅在SINGLE_LEVEL_ONLY模式下检查）
                boolean levelFinished = _m_hadDrawFreeStep >= levelRef.last_step
                        && _m_hadDrawPayStep >= levelRef.last_step && levelRef.last_step > 0;

                if (mode == RewardCollectMode.SINGLE_LEVEL_ONLY && levelFinished)
                {
                    prevLevelLastStep = levelRef.last_step;
                    continue;
                }

                // 计算该等级包含的阶段范围
                int levelStartStep = prevLevelLastStep + 1;
                int levelEndStep = levelRef.last_step < 0 ? Integer.MAX_VALUE : levelRef.last_step;

                // 计算该等级下可领取的阶段范围
                int canDrawStartStep = Math.max(levelStartStep, startStep);
                int canDrawEndStep = Math.min(levelEndStep, endStep);

                if (canDrawStartStep > canDrawEndStep)
                {
                    prevLevelLastStep = levelRef.last_step;
                    continue;
                }

                // 获取该等级下的阶段配置
                List<RefActivityFundStep> stepRefList =
                        RefActivityFundStep.getMgr().getStepListByRange(getFundId(), canDrawStartStep, canDrawEndStep);

                if (stepRefList == null || stepRefList.isEmpty())
                {
                    prevLevelLastStep = levelRef.last_step;
                    continue;
                }

                // 检查是否拥有该等级的凭证道具（提前到外层，避免重复调用）
                boolean hasDistinguishItem = hasDistinguishItem(levelRef);

                // 遍历该等级下的所有阶段，收集奖励
                for (RefActivityFundStep stepRef : stepRefList)
                {
                    // 收集免费档奖励
                    if (stepRef.step > _m_hadDrawFreeStep)
                    {
                        if (stepRef.free_reward_item_list != null && !stepRef.free_reward_item_list.isEmpty())
                            rewardList.addAll(stepRef.free_reward_item_list);

                        newMaxFreeStep = Math.max(newMaxFreeStep, stepRef.step);
                        hasProgress = true;
                    }

                    // 收集付费档奖励（需要凭证）
                    if (stepRef.step > _m_hadDrawPayStep && hasDistinguishItem)
                    {
                        if (stepRef.pay_reward_item_list != null && !stepRef.pay_reward_item_list.isEmpty())
                            rewardList.addAll(stepRef.pay_reward_item_list);

                        newMaxPayStep = Math.max(newMaxPayStep, stepRef.step);
                        hasProgress = true;
                    }
                }

                prevLevelLastStep = levelRef.last_step;

                // 如果是单等级模式且有进度，处理完后退出
                if (mode == RewardCollectMode.SINGLE_LEVEL_ONLY && hasProgress)
                    break;
            }

            // 5. 如果没有新的进度
            if (!hasProgress)
                return ResultOne.failed(ActivityFundErr.ALREADY_DRAWN);

            // 6. 更新已领取阶段记录
            _m_hadDrawFreeStep = newMaxFreeStep;
            _m_hadDrawPayStep = newMaxPayStep;

            // 7. 保存到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("drawn_free_steps", _m_hadDrawFreeStep);
                updateValue.addValueObj("drawn_paid_steps", _m_hadDrawPayStep);
                getBM().getBM(PlayerActivityFundBO.class).update("id", _m_dbId, updateValue);
            }

            // 8. 返回奖励列表
            return ResultOne.succ(rewardList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 奖励收集模式枚举
     */
    private enum RewardCollectMode
    {
        // 仅处理首个未完成等级（drawAllAvailableRewards使用）
        SINGLE_LEVEL_ONLY,

        // 遍历所有等级（sendUnclaimedRewardsEmail使用）
        ALL_LEVELS
    }

    /**
     * 记录活动基金奖励领取日志
     * <p>
     * 执行流程：
     * 1. 创建日志BO对象
     * 2. 设置日志字段
     * 3. 使用CommLogDB记录日志
     *
     * @param _hadDrawFreeStepBefore 领取前免费档已领取阶段
     * @param _hadDrawPayStepBefore  领取前付费档已领取阶段
     * @param _hadDrawFreeStepAfter 领取后免费档已领取阶段
     * @param _hadDrawPayStepAfter  领取后付费档已领取阶段
     * @param _context 操作上下文
     */
    private void recordDrawLog(int _hadDrawFreeStepBefore, int _hadDrawPayStepBefore,
                               int _hadDrawFreeStepAfter, int _hadDrawPayStepAfter,
                               NPPlayerContext _context)
    {
        try
        {
            // 创建日志BO对象
            LogActivityFundDrawBO logBO = new LogActivityFundDrawBO();

            // 设置玩家CID
            logBO.setCid(getBM(), _m_comp.getUserData().getCid());

            // 设置基金ID
            logBO.setFundId(getBM(), getFundId());

            // 设置活动实例ID
            logBO.setActivityInstanceId(getBM(), _m_activityInstanceId);

            // 设置领取前状态
            logBO.setHadDrawFreeStepBefore(getBM(), _hadDrawFreeStepBefore);
            logBO.setHadDrawPayStepBefore(getBM(), _hadDrawPayStepBefore);

            // 设置领取后状态
            logBO.setHadDrawFreeStepAfter(getBM(), _hadDrawFreeStepAfter);
            logBO.setHadDrawPayStepAfter(getBM(), _hadDrawPayStepAfter);

            // 使用CommLogDB记录日志
            CommLogDB.log(getBM(), logBO, _context);
        }
        catch (Exception e)
        {
            CommLog.error("ActivityFundInfo.recordDrawLog - failed to record draw log: cid={}, fundId={}, error={}",
                    _m_comp.getUserData().getCid(), getFundId(), e.getMessage(), e);
        }
    }

    /**
     * 记录分数变动日志
     *
     * @param _scoreType 分数类型(1=公式分数, 2=任务分数)
     * @param _scoreBefore 变动前分数
     * @param _scoreAfter 变动后分数
     * @param _context 操作上下文
     */
    private void recordScoreLog(int _scoreType, long _scoreBefore, long _scoreAfter, NPPlayerContext _context)
    {
        try
        {
            // 创建日志BO对象
            LogActivityFundScoreBO logBO = new LogActivityFundScoreBO();

            // 设置玩家CID
            logBO.setCid(getBM(), _m_comp.getUserData().getCid());

            // 设置基金ID
            logBO.setFundId(getBM(), getFundId());

            // 设置活动实例ID
            logBO.setActivityInstanceId(getBM(), _m_activityInstanceId);

            // 设置分数类型
            logBO.setScoreType(getBM(), _scoreType);

            // 设置变动前后分数
            logBO.setScoreBefore(getBM(), _scoreBefore);
            logBO.setScoreAfter(getBM(), _scoreAfter);

            // 设置变动值
            logBO.setChangeValue(getBM(), _scoreAfter - _scoreBefore);

            // 使用CommLogDB记录日志
            CommLogDB.log(getBM(), logBO, _context);
        }
        catch (Exception e)
        {
            CommLog.error("ActivityFundInfo.recordScoreLog - failed to record score log: cid={}, fundId={}, scoreType={}, error={}",
                    _m_comp.getUserData().getCid(), getFundId(), _scoreType, e.getMessage(), e);
        }
    }

}
