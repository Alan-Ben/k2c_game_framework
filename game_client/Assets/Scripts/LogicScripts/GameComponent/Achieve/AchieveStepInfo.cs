namespace GOE
{
    /// <summary>
    /// 成就步骤信息
    /// </summary>
    public class AchieveStepInfo
    {
        private long _m_lAchieveId;//成就id
        private AchieveStepRefObj _m_stepRefObj;//成就步骤配置
        private AchieveInfo _m_achieveInfo;//成就信息


        /// <summary>
        /// 成就id
        /// </summary>
        public long achieveId { get { return _m_lAchieveId; } }
        /// <summary>
        /// 成就信息
        /// </summary>
        public AchieveInfo achieveInfo { get { return _m_achieveInfo; } }
        /// <summary>
        /// 成就步骤配置
        /// </summary>
        public AchieveStepRefObj stepRefObj { get => _m_stepRefObj; }
        /// <summary>
        /// 成就步骤id
        /// </summary>
        public int step { get { return _m_stepRefObj == null ? 0 : _m_stepRefObj.step; } }


        public AchieveStepInfo(AchieveInfo _achieveInfo, AchieveStepRefObj _stepRefObj)
        {
            if (_stepRefObj == null)
            {
                Debug.LogError("【成就】初始化成就步骤数据错误 _stepRefObj = null");
                return;
            }

            _m_achieveInfo = _achieveInfo;
            _m_stepRefObj = _stepRefObj;
            _m_lAchieveId = _stepRefObj.achieve_id;
        }

        /// <summary>
        /// 是否已经达到成就计数
        /// </summary>
        /// <returns></returns>
        public bool isFull()
        {
            if (_m_achieveInfo == null || _m_stepRefObj == null)
                return false;

            return _m_achieveInfo.curStepCount >= _m_stepRefObj.process_count;
        }

        /// <summary>
        /// 是否已经领取奖励
        /// </summary>
        /// <returns></returns>
        public bool hasGetReward()
        {
            if (_m_achieveInfo == null || _m_achieveInfo.hasDrawStepList == null || _m_stepRefObj == null)
                return false;

            return _m_achieveInfo.hasDrawStepList.Contains(_m_stepRefObj.step);
        }

        /// <summary>
        /// 当前可领取状态
        /// </summary>
        /// <returns></returns>
        public ENPCommonGetStat getRewardState()
        {
            if (hasGetReward()) 
                return ENPCommonGetStat.HAS_GET;

            if (isFull())
                return ENPCommonGetStat.CAN_GET;

            return ENPCommonGetStat.CAN_NOT_GET;
        }
    }
}