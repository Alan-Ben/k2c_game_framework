using Common.ActivityObj;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励信息
    /// </summary>
    public class ActivityStepRewardInfo
    {
        // 活动实例id
        private long _m_lActivityInstanceId;
        // 阶段奖励设置id
        private long _m_stepRewardSetId;
        // 阶段奖励设置配置
        private ActivityStepRewardSetRefObj _m_stepRewardSetRef;
        // set上获取的分数
        private long _m_lSetScore;
        // 所有分数(set分数+事件任务分数)
        private long _m_lTotalScore;
        // 已领取的奖励列表
        [NotNull]private List<int> _m_lHadDrawStepList = new List<int>();
        // 事件任务数据列表
        private List<ActivityStepRewardEventTaskInfo> _m_lEventTaskInfoList = new List<ActivityStepRewardEventTaskInfo>();

        /// <summary>
        /// 活动实例id
        /// </summary>
        public long activityInstanceId => _m_lActivityInstanceId;
        /// <summary>
        /// 阶段奖励设置id
        /// </summary>
        public long stepRewardSetId => _m_stepRewardSetId;
        /// <summary>
        /// 阶段奖励设置配置
        /// </summary>
        public ActivityStepRewardSetRefObj stepRewardSetRef => _m_stepRewardSetRef;
        public long totalScore => _m_lTotalScore;
        /// <summary>
        /// 是否显示在阶段奖励界面
        /// </summary>
        public bool isShowInStepRewardWnd => _m_stepRewardSetRef != null && _m_stepRewardSetRef.is_show_in_step_reward_wnd;

        /// <summary>
        /// 事件任务数据字典
        /// </summary>
        public List<ActivityStepRewardEventTaskInfo> eventTaskInfoList => _m_lEventTaskInfoList;


        public ActivityStepRewardInfo(long _activityInstanceId, long _stepRewardSetId)
        {
            _m_lActivityInstanceId = _activityInstanceId;
            _m_stepRewardSetId = _stepRewardSetId;
            _m_stepRewardSetRef = GRefdataCoreMgr.instance.stepRewardSetRefCore.getRef(_m_stepRewardSetId);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_stepRewardInfo"></param>
        public void updateInfo(Activity_StepRewardInfo _stepRewardInfo)
        {
            if (_stepRewardInfo == null)
                return;

            _m_lSetScore = _m_lTotalScore = _stepRewardInfo.getScore();
            _m_lHadDrawStepList = _stepRewardInfo.getHadDrawStepList();

            // 更新事件任务数据
            if(_m_lEventTaskInfoList == null)
                _m_lEventTaskInfoList = new List<ActivityStepRewardEventTaskInfo>();
            _m_lEventTaskInfoList.Clear();
            List<Activity_StepRewardEventTaskInfo> eventTaskList = _stepRewardInfo.getEventTaskList();
            if (eventTaskList != null)
            {
                for (int i = 0; i < eventTaskList.Count; i++)
                {
                    Activity_StepRewardEventTaskInfo eventTaskInfo = eventTaskList[i];
                    if (eventTaskInfo == null)
                        continue;

                    _m_lEventTaskInfoList.Add(new ActivityStepRewardEventTaskInfo(eventTaskInfo));
                    _m_lTotalScore += eventTaskInfo.getScore();
                }
            }
        }
        
        /// <summary>
        /// 获取可领奖状态
        /// </summary>
        /// <param name="_stepRef"></param>
        /// <returns></returns>
        public EStepRewardState getStepRewardState(GActivityStepRewardRefObj _stepRef)
        {
            if(_stepRef == null)
                return EStepRewardState.None;
            
            if (_m_lHadDrawStepList.Contains((int)_stepRef.step))
                return EStepRewardState.AlreadyGet;

            return _stepRef.complete_count <= totalScore ? EStepRewardState.CanGet : EStepRewardState.None;
        }

        /// <summary>
        /// 是否成就步骤已经全部完成
        /// </summary>
        /// <returns></returns>
        public bool isAllDone()
        {
            List<GActivityStepRewardRefObj> stepRewardRefList = GRefdataCoreMgr.instance.getStepRewardRefList(_m_stepRewardSetId);
            if (stepRewardRefList == null || stepRewardRefList.Count <= 0)
                return true;

            for (int i = 0; i < stepRewardRefList.Count; i++)
            {
                //只要有一个不是已领取，就不算全部完成
                if (getStepRewardState(stepRewardRefList[i]) != EStepRewardState.AlreadyGet)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取可领取奖励数量
        /// </summary>
        /// <returns></returns>
        public int getCanGetRewardCount()
        {
            List<GActivityStepRewardRefObj> rewardRefList = GRefdataCoreMgr.instance.getStepRewardRefList(_m_stepRewardSetId);
            if (rewardRefList == null || rewardRefList.Count <= 0)
                return 0;
            int count = 0;
            
            foreach (GActivityStepRewardRefObj stepRewardRefObj in rewardRefList)
            {
                if(stepRewardRefObj != null && stepRewardRefObj.complete_count <= totalScore && !_m_lHadDrawStepList.Contains((int)stepRewardRefObj.step))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 获取第一个未领取的阶段奖励配置
        /// </summary>
        /// <returns></returns>
        public GActivityStepRewardRefObj getFirstNotGetRewardStep()
        {
            GActivityStepRewardRefObj curStepRef = null;
            List<GActivityStepRewardRefObj> stepRewardRefList = GRefdataCoreMgr.instance.getStepRewardRefList(_m_stepRewardSetId);
            if (stepRewardRefList == null || stepRewardRefList.Count <= 0)
                return curStepRef;

            stepRewardRefList.Sort((_a,_b)=>_a.step.CompareTo(_b.step));
            //获取第一个未领取的阶段奖励
            for (int i = 0; i < stepRewardRefList.Count; i++)
            {
                if (stepRewardRefList[i] != null && !_m_lHadDrawStepList.Contains(stepRewardRefList[i].step))
                {
                    curStepRef = stepRewardRefList[i];
                    break;
                }
            }
            //未取到数据，取最后一个已领取的阶段奖励
            if(curStepRef == null)
                curStepRef = stepRewardRefList.GetLast();
            return curStepRef;
        }

        /// <summary>
        /// 设置阶段奖励已领取
        /// </summary>
        /// <param name="_step"></param>
        public void setStepHasGet(int _step)
        {
            if(!_m_lHadDrawStepList.Contains(_step))
                _m_lHadDrawStepList.Add(_step);
        }

        /// <summary>
        /// 设置所有可领取奖励为已领取
        /// </summary>
        public void setAllCanGetStepHasGet()
        {
            List<GActivityStepRewardRefObj> rewardRefList = GRefdataCoreMgr.instance.getStepRewardRefList(_m_stepRewardSetId);
            if (rewardRefList == null || rewardRefList.Count <= 0)
                return ;
            int count = 0;
            
            foreach (GActivityStepRewardRefObj stepRewardRefObj in rewardRefList)
            {
                if(stepRewardRefObj.complete_count <= totalScore && !_m_lHadDrawStepList.Contains(stepRewardRefObj.step))
                    _m_lHadDrawStepList.Add(stepRewardRefObj.step);
            }
        }
        
        /// <summary>
        /// 更新分数
        /// </summary>
        /// <param name="_score"></param>
        public void updateScore(long _score)
        {
            _m_lTotalScore -= _m_lSetScore;
            _m_lSetScore = _score;
            _m_lTotalScore += _m_lSetScore;
        }

        /// <summary>
        /// 更新事件任务分数
        /// </summary>
        /// <param name="_eventTaskId">事件任务ID</param>
        /// <param name="_score">分数</param>
        public void updateEventTaskScore(long _eventTaskId, long _score)
        {
            ActivityStepRewardEventTaskInfo taskInfo = getEventTaskInfo(_eventTaskId);
            long prevScore = taskInfo?.score ?? 0;//更新前分数
            if (taskInfo == null)
            {
                taskInfo = new ActivityStepRewardEventTaskInfo(_eventTaskId, _score);

                if (_m_lEventTaskInfoList == null)
                    _m_lEventTaskInfoList = new List<ActivityStepRewardEventTaskInfo>();
                _m_lEventTaskInfoList.Add(taskInfo);
                
                _m_lTotalScore += _score;
            }
            else
            {
                _m_lTotalScore -= taskInfo.score;
                taskInfo.updateScore(_score);
                _m_lTotalScore += taskInfo.score;
            }

            if (taskInfo.score > prevScore)//若更新后分数大于更新前分数，显示完成提示
            {
                if(null == taskInfo.eventTaskRef || taskInfo.eventTaskRef.center_tip_id <= 0 || 
                   (taskInfo.eventTaskRef.process_limit > 0 && taskInfo.score > taskInfo.eventTaskRef.process_limit))// 当任务达到获取积分上限时，不再弹出侧边提示
                    return;

                NPGUIAddSceneCenterTip.instance.showIconTextTip(taskInfo.eventTaskRef.icon, 
                    TextTranslate.instance.getLanguage(taskInfo.eventTaskRef.task_name, taskInfo.eventTaskRef.task_name_args), taskInfo.eventTaskRef.center_tip_id);
            }
        }

        /// <summary>
        /// 获取事件任务信息
        /// </summary>
        /// <param name="_eventTaskId">事件任务ID</param>
        /// <returns>事件任务信息，若不存在则返回null</returns>
        public ActivityStepRewardEventTaskInfo getEventTaskInfo(long _eventTaskId)
        {
            if(_m_lEventTaskInfoList == null)
                return null;
            
            return _m_lEventTaskInfoList.Find(info => info != null && info.eventTaskId == _eventTaskId);
        }
        
        /// <summary>
        /// 阶梯奖励排序：可领取的在前，已领取的在后，按step从小到大排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int sort(GActivityStepRewardRefObj a, GActivityStepRewardRefObj b)
        {
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            
            // 获取奖励状态
            EStepRewardState stateA = getStepRewardState(a);
            EStepRewardState stateB = getStepRewardState(b);
            
            // 可领取的排在最前面
            if (stateA == EStepRewardState.CanGet && stateB != EStepRewardState.CanGet)
                return -1;
            if (stateB == EStepRewardState.CanGet && stateA != EStepRewardState.CanGet)
                return 1;
            
            // 已领取的排在最后面
            if (stateA == EStepRewardState.AlreadyGet && stateB != EStepRewardState.AlreadyGet)
                return 1;
            if (stateB == EStepRewardState.AlreadyGet && stateA != EStepRewardState.AlreadyGet)
                return -1;
            
            // 相同状态下按step从小到大排序
            return a.step.CompareTo(b.step);
        }
    }
}