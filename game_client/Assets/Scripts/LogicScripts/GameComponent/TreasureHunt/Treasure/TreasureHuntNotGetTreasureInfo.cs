namespace GOE
{
    /// <summary>
    /// 未获取的奇物信息
    /// </summary>
    public class TreasureHuntNotGetTreasureInfo : _ITreasureHuntTreasureInfo
    {
        private TreasureHuntTreasureRefObj _m_treasureRefObj;
        private TreasureHuntTreasureOutputRefObj _m_treasureOutputRefObj;
        private _ITreasureHuntSkillInfo _m_skillInfo;

        public TreasureHuntNotGetTreasureInfo(long _treasureId)
        {
            _m_treasureRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.getRef(_treasureId);
            _m_treasureOutputRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureOutputRefCore.getRef(_treasureId);
            
            if (_m_treasureRefObj != null)
            {
                _m_skillInfo = new TreasureHuntTreasureSkillInfo(_m_treasureRefObj.skill_id, false, 0, _m_treasureRefObj.upgrade_cost);
            }
        }

        public TreasureHuntNotGetTreasureInfo(TreasureHuntTreasureRefObj _treasureRefObj)
        {
            _m_treasureRefObj = _treasureRefObj;
            _m_treasureOutputRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureOutputRefCore.getRef(_m_treasureRefObj?.id ?? 0);
            if (_m_treasureRefObj != null)
            {
                _m_skillInfo = new TreasureHuntTreasureSkillInfo(_m_treasureRefObj.skill_id, false, 0, _m_treasureRefObj.upgrade_cost);
            }
        }
        
        public long treasureId { get { return _m_treasureRefObj?.id ?? 0; } }
        public TreasureHuntTreasureRefObj treasureRefObj { get { return _m_treasureRefObj; } }
        public TreasureHuntTreasureOutputRefObj treasureOutputRefObj { get { return _m_treasureOutputRefObj; } }
        public ETreasureHuntTreasureState treasureState { get { return ETreasureHuntTreasureState.NOT_GET; } }
        public long gainTimeMs { get { return 0; } }
        public _ITreasureHuntSkillInfo skillInfo { get { return _m_skillInfo; } }
    }
}