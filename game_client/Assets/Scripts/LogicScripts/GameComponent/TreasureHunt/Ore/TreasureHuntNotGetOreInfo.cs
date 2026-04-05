namespace GOE
{
    /// <summary>
    /// 未获取的矿石信息
    /// </summary>
    public class TreasureHuntNotGetOreInfo : _ITreasureHuntOreInfo
    {
        private TreasureHuntOreRefObj _m_oreRefObj;
        
        private _ITreasureHuntSkillInfo _m_normalSkillInfo;
        private _ITreasureHuntSkillInfo _m_advanceSkillInfo;
        
        public TreasureHuntNotGetOreInfo(long _oreId)
        {
            _m_oreRefObj = GRefdataCoreMgr.instance.treasureHuntOreRefCore.getRef(_oreId);

            if (_m_oreRefObj != null)
            {
                _m_normalSkillInfo = new TreasureHuntCommonSkillInfo(_m_oreRefObj.normal_skill_id, false, 0, null, 0);
                _m_advanceSkillInfo = new TreasureHuntCommonSkillInfo(_m_oreRefObj.advanced_skill_id, false, 0, null, 0);
            }
        }

        public TreasureHuntNotGetOreInfo(TreasureHuntOreRefObj _oreRefObj)
        {
            _m_oreRefObj = _oreRefObj;
            
            if (_m_oreRefObj != null)
            {
                _m_normalSkillInfo = new TreasureHuntCommonSkillInfo(_m_oreRefObj.normal_skill_id, false, 0, null, 0);
                _m_advanceSkillInfo = new TreasureHuntCommonSkillInfo(_m_oreRefObj.advanced_skill_id, false, 0, null, 0);
            }
        }

        public long oreId { get { return _m_oreRefObj?.id ?? 0; } }
        public TreasureHuntOreRefObj oreRefObj { get { return _m_oreRefObj; } }
        public ETreasureHuntOreState oreState { get { return ETreasureHuntOreState.NOT_GET; } }
        public int num { get { return 0; } }
        public int mass { get { return 0; } }
        public long getTimeMs { get { return 0; } }
        public _ITreasureHuntSkillInfo normalSkillInfo { get { return _m_normalSkillInfo; } }
        public _ITreasureHuntSkillInfo advanceSkillInfo { get { return _m_advanceSkillInfo; } }
    }
}