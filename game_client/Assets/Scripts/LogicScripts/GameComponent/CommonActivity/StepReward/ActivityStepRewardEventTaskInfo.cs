using Common.ActivityObj;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励事件任务信息
    /// </summary>
    public class ActivityStepRewardEventTaskInfo
    {
        // 事件配置ID
        private long _m_lEventTaskId;
        // 分数
        private long _m_lScore;
        // 配置引用(延迟获取)
        private ActivityStepRewardSetEventTaskRefObj _m_eventTaskRef;

        /// <summary>
        /// 事件配置ID
        /// </summary>
        public long eventTaskId => _m_lEventTaskId;

        /// <summary>
        /// 分数
        /// </summary>
        public long score => _m_lScore;

        /// <summary>
        /// 获取事件任务配置引用
        /// </summary>
        public ActivityStepRewardSetEventTaskRefObj eventTaskRef
        {
            get
            {
                if (_m_eventTaskRef == null || _m_lEventTaskId != _m_eventTaskRef.id)
                    _m_eventTaskRef = GRefdataCoreMgr.instance.stepRewardSetEventTaskRefCore.getRef(_m_lEventTaskId);
                return _m_eventTaskRef;
            }
        }

        public ActivityStepRewardEventTaskInfo(long _eventTaskId, long _score)
        {
            _m_lEventTaskId = _eventTaskId;
            _m_lScore = _score;
        }

        public ActivityStepRewardEventTaskInfo(Activity_StepRewardEventTaskInfo _info)
        {
            if (_info == null)
                return;

            _m_lEventTaskId = _info.getEventTaskId();
            _m_lScore = _info.getScore();
        }

        /// <summary>
        /// 更新分数
        /// </summary>
        /// <param name="_score">新的分数</param>
        public void updateScore(long _score)
        {
            _m_lScore = _score;
        }
    }
}