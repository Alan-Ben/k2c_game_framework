using System;
using System.Collections.Generic;
using ALPackage;
using Common.SimpleActivityObj;
using GC2GS.p033_SimpleActivityOp;
using GS2GC.p033_SimpleActivityOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class SevenDayGoalsData
    {
        // 所有七日目标的任务数据，按天数排列，数组 0 代表第 1 天，1 代表第 2 天，字典是任务 id 和任务数据的映射
        [ItemNotNull, NotNull] private readonly List<List<SevenDayGoalsTaskInfo>> _m_taskInfoList;
        [NotNull] private readonly Dictionary<long, SevenDayGoalsTaskInfo> _m_taskInfoDic;
        // 七日目标的额外计数数据，key 是任务 id，value 是额外计数
        [NotNull] private readonly Dictionary<long, long> _m_taskExtraCountDic;
        // 已领取奖励的阶段 id 集合，阶段是依靠积分积累的
        [NotNull] private readonly HashSet<long> _m_hadDrawStepRewardIds;
        // 红点提示处理器
        [NotNull] private readonly RedTipDealerNew _m_redTipDealer;

        // 当前阶段积分
        private long _m_score;

        private _ABaseActivityInfo _m_activityInfo;

        // 初始化序列号
        private bool _m_isInit;
        private int _m_initSerialize;
        
        
        public SevenDayGoalsData() 
        {
            _m_taskInfoList = new List<List<SevenDayGoalsTaskInfo>>(7);
            _m_taskInfoDic = new Dictionary<long, SevenDayGoalsTaskInfo>();
            _m_taskExtraCountDic = new Dictionary<long, long>();
            _m_hadDrawStepRewardIds = new HashSet<long>();
            _m_redTipDealer = new RedTipDealerNew(this);
        }
        
        
        /// <summary>
        /// 当任务数据发生变化
        /// </summary>
        public event Action<long> onTaskChg;
        /// <summary>
        /// 当阶段奖励数据发生变化
        /// </summary>
        public event Action<long> onStepRewardChg;
        /// <summary>
        /// 当积分发生变化
        /// </summary>
        public event Action onScoreChg;
        
        /// <summary>
        /// 当前的阶段总积分
        /// </summary>
        public long score { get { return _m_score; } }
        public bool isInit { get { return _m_isInit; } }
        public _ABaseActivityInfo activityInfo { get { return _m_activityInfo; } }
        /// <summary>
        /// 红点管理器
        /// </summary>
        public RedTipDealerNew redTipDealer { get { return _m_redTipDealer; } }


        public void init(_ABaseActivityInfo _activityInfo)
        {
            if (_m_isInit)
                return;

            _m_isInit = true;
            _m_activityInfo = _activityInfo;
            int serialize = _m_initSerialize;
            NPGSClientListener.sendRequestByLog(new GC2GS_033_004_ReqSevenDayGoalsInfo(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_033_004_RetSevenDayGoalsInfo>((_isSuc, _msg) =>
                {
                    if (serialize != _m_initSerialize)
                        return;
                    
                    _initData(_msg);
                }));
            
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }
        public void discard()
        {
            if (!_m_isInit)
                return;

            _m_isInit = false;
            _m_initSerialize = ALSerializeOpMgr.next();

            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            
            foreach (List<SevenDayGoalsTaskInfo> dayTasks in _m_taskInfoList)
            {
                foreach (SevenDayGoalsTaskInfo taskInfo in dayTasks)
                {
                    taskInfo.unregisterTaskChg(_onTaskChg);
                    taskInfo.discard();
                }
            }
            _m_taskInfoList.Clear();
            _m_taskInfoDic.Clear();

            _m_taskExtraCountDic.Clear();
            _m_hadDrawStepRewardIds.Clear();

            _m_redTipDealer.clear();

            _m_activityInfo = null;
            _m_score = 0;
        }
        
        
        /// <summary>
        /// 是否已经领取了某个任务的奖励了
        /// </summary>
        [Pure]
        public bool isTaskHadDrawReward(long _taskId)
        {
            if (_m_taskInfoDic.TryGetValue(_taskId, out SevenDayGoalsTaskInfo taskInfo) && null != taskInfo)
            {
                return taskInfo.hadDrawReward;
            }
            
            return false;
        }
        /// <summary>
        /// 是否已经领取了某个阶段的奖励了
        /// </summary>
        [Pure]
        public bool isStepRewardHadDraw(long _stepId)
        {
            return _m_hadDrawStepRewardIds.Contains(_stepId);
        }
        /// <summary>
        /// 获取现在是第几天
        /// </summary>
        [Pure]
        public int getNowDay()
        {
            return (int)NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS);
        }
        /// <summary>
        /// 判断指定任务是否可以获得奖励
        /// </summary>
        [Pure]
        public bool canTaskGetReward(long _taskId)
        {
            if (!_m_taskInfoDic.TryGetValue(_taskId, out SevenDayGoalsTaskInfo taskInfo))
                return false;

            int day = getNowDay();
            SevenDayGoalsTaskRewardRefObj refObj = taskInfo.refObj;
            // 如果任务的天数大于当前天数，说明未解锁
            if (refObj.day > day)
                return false;
                
            // 如果已经领奖了就返回 false
            if (isTaskHadDrawReward(_taskId))
                return false;
                
            // 如果完成的数量不达标返回 false
            if (taskInfo.getCount() + _getTaskExtraCount(_taskId) < refObj.goal_count)
                return false;

            // 全通过就返回 true
            return true;
        }
        /// <summary>
        /// 获取指定任务的完成数量
        /// </summary>
        [Pure]
        public long getTaskCount(long _taskId)
        {
            if (!_m_taskInfoDic.TryGetValue(_taskId, out SevenDayGoalsTaskInfo taskInfo))
                return 0;

            return taskInfo.getCount() + _getTaskExtraCount(_taskId);
        }
        /// <summary>
        /// 获取可领奖的天数
        /// </summary>
        [Pure]
        public int getCanGetRewardDay()
        {
            int nowDay = getNowDay();
            for (int i = 1; i <= nowDay; i++)
            {
                List<SevenDayGoalsTaskRewardRefObj> taskList = GRefdataCoreMgr.instance.getSevenDayGoalsDayTasks(i);
                if (taskList == null)
                    continue;

                foreach (SevenDayGoalsTaskRewardRefObj task in taskList)
                {
                    if (canTaskGetReward(task.id))
                        return i;
                }
            }
            return nowDay;
        }


        private void _initData(GS2GC_033_004_RetSevenDayGoalsInfo _msg)
        {
            if (_msg == null)
                return;

            // 初始化任务列表
            List<SevenDayGoals_TaskInfo> taskList = _msg.getTaskList();
            if (taskList != null)
            {
                foreach (SevenDayGoals_TaskInfo taskInfo in taskList)
                {
                    if (taskInfo == null)
                        continue;

                    _m_taskExtraCountDic[taskInfo.getTaskId()] = taskInfo.getExtraCount();
                }
            }
            
            // 初始化已领取奖励的任务 id 集合
            List<long> hadDrawRewardList = _msg.getHadDrawRewardList();
            
            // 初始化已领取奖励的阶段 id 集合
            List<long> hadDrawStepRewardList = _msg.getHadDrawStepRewardList();
            if (hadDrawStepRewardList != null)
            {
                foreach (long id in hadDrawStepRewardList)
                    _m_hadDrawStepRewardIds.Add(id);
            }

            // 初始化当前积分
            _m_score = _msg.getScore();

            int day = GRefdataCoreMgr.instance.getSevenDayGoalsMaxDay();
            for (int i = 0; i < day; i++)
            {
                List<SevenDayGoalsTaskRewardRefObj> taskRefList = GRefdataCoreMgr.instance.getSevenDayGoalsDayTasks(i + 1);
                List<SevenDayGoalsTaskInfo> dayTasks = new List<SevenDayGoalsTaskInfo>(taskRefList?.Count ?? 0);
                _m_taskInfoList.Add(dayTasks);
                if (taskRefList != null)
                {
                    foreach (SevenDayGoalsTaskRewardRefObj refObj in taskRefList)
                    {
                        SevenDayGoalsTaskInfo taskInfo = new SevenDayGoalsTaskInfo(refObj, hadDrawRewardList.Contains(refObj._refId));
                        dayTasks.Add(taskInfo);
                        _m_taskInfoDic[taskInfo.taskId] = taskInfo;

                        taskInfo.init();
                        taskInfo.registerTaskChg(_onTaskChg);
                    }
                }
            }
            
            _m_redTipDealer.init();
            WinMsg.SendMsg(WinMsgType.ON_SEVEN_DAY_GOAL_INIT_DONE);
        }
        private void _onPlayerParamChg(object[] _params)
        {
            if (_params is not { Length: > 0 } || _params[0] is not int paramIndex)
                return;

            ENPPlayerParam playerParam = (ENPPlayerParam)paramIndex;
            if (playerParam != ENPPlayerParam.SERVER_START_DAYS)
                return;
            
            _m_redTipDealer.refreshAll();
        }
        private void _onActivityStateChg(object[] _params)
        {
            if (_m_activityInfo == null)
                return;
            
            if (_params is not { Length: > 3 } || _params[1] is not long activityInstanceId)
                return;

            if (activityInstanceId != _m_activityInfo.instanceId)
                return;

            // 活动状态改变了，刷新红点
            _m_redTipDealer.refreshAll();
        }


        internal void _onSevenDayGoalTaskChg(GS2GC_033_103_OnSevenDayGoalTaskChg _msg)
        {
            if (!_m_isInit)
                return;
            
            SevenDayGoals_TaskInfo serverTaskInfo = _msg?.getTaskInfo();
            if (serverTaskInfo == null)
                return;
            
            long taskId = serverTaskInfo.getTaskId();
            _m_taskExtraCountDic[taskId] = serverTaskInfo.getExtraCount();
            _m_redTipDealer.refreshTaskRedTipByTaskId(taskId);
            onTaskChg?.Invoke(taskId);
        }
        internal void _onSevenDayGoalScoreChg(GS2GC_033_104_OnSevenDayGoalScoreChg _msg)
        {
            if (_msg == null || !_m_isInit)
                return;

            _m_score = _msg.getScore();
            _m_redTipDealer.refreshStepRedTip();
            onScoreChg?.Invoke();
        }
        internal void _onSevenDayGoalRewardDraw(GS2GC_033_107_OnSevenDayGoalRewardDraw _msg)
        {
            if (_msg == null|| !_m_isInit)
                return;

            long taskId = _msg.getRefId();
            
            if (_m_taskInfoDic.TryGetValue(taskId, out SevenDayGoalsTaskInfo taskInfo) && null != taskInfo)
            {
                taskInfo.setHadDrawReward();
            }
            
            _m_redTipDealer.refreshTaskRedTipByTaskId(taskId);
            onTaskChg?.Invoke(taskId);
        }
        internal void _onSevenDayGoalStepRewardDraw(GS2GC_033_108_OnSevenDayGoalStepRewardDraw _msg)
        {
            if (_msg == null || !_m_isInit)
                return;
            
            long stepId = _msg.getRefId();
            _m_hadDrawStepRewardIds.Add(stepId);
            _m_redTipDealer.refreshStepRedTip();
            onStepRewardChg?.Invoke(stepId);
        }


        // 任务数据改变的回调，注册给任务数据后由任务数据自己触发
        private void _onTaskChg(long _taskId)
        {
            _m_redTipDealer.refreshTaskRedTipByTaskId(_taskId);
            onTaskChg?.Invoke(_taskId);
        }
        // 获取某个任务的额外计数
        private long _getTaskExtraCount(long _taskId)
        {
            if (_m_taskExtraCountDic.TryGetValue(_taskId, out long extraCount))
                return extraCount;

            return 0;
        }
    }
}