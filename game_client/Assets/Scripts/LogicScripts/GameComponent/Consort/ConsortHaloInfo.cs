namespace GOE
{
    /// <summary>
    /// 妃子星辉信息
    /// </summary>
    public class ConsortHaloInfo
    {
        private bool _m_bIsUnlock;//是否解锁星辉
        private ConsortHaloLvlRefObj _m_rNowHaloLvlRefObj;//当前星辉等级

        public bool isUnlock { get { return _m_bIsUnlock; } }
        public ConsortHaloLvlRefObj nowHaloLvlRefObj { get { return _m_rNowHaloLvlRefObj; } }
        public int level { get { return _m_rNowHaloLvlRefObj?.level ?? 0; } }
        
        public ConsortHaloInfo(bool _isUnlock, ConsortHaloLvlRefObj _haloLvlRefObj)
        {
            _m_bIsUnlock = _isUnlock;
            _m_rNowHaloLvlRefObj = _haloLvlRefObj;
        }

        /// <summary>
        /// 更新星辉解锁状态
        /// </summary>
        public void updateHaloUnlockState(bool _isUnlock)
        {
            _m_bIsUnlock = _isUnlock;
        }
        
        /// <summary>
        /// 更新星辉等级
        /// </summary>
        /// <param name="_haloLvlRefObj"></param>
        public void updateHaloLvl(ConsortHaloLvlRefObj _haloLvlRefObj)
        {
            _m_rNowHaloLvlRefObj = _haloLvlRefObj;
        }
    }
}