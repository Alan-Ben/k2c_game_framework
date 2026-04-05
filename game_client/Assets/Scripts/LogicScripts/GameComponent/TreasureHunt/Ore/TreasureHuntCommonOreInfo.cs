namespace GOE
{
    public class TreasureHuntCommonOreInfo : _ITreasureHuntOreInfo
    {
        private TreasureHuntOreRefObj _m_oreRefObj;
        private ETreasureHuntOreState _m_eOreState;
        private int _m_iNum;
        private int _m_iMass;
        private long _m_lGetTimeMs;
        private _ITreasureHuntSkillInfo _m_normalSkillInfo;
        private _ITreasureHuntSkillInfo _m_advanceSkillInfo;
        
        public TreasureHuntCommonOreInfo(long _oreId, ETreasureHuntOreState _oreState, int _num, int _mass, long _getTimeMs, _ITreasureHuntSkillInfo _normalSkillInfo = null, _ITreasureHuntSkillInfo _advanceSkillInfo = null)
        {
            _m_oreRefObj = GRefdataCoreMgr.instance.treasureHuntOreRefCore.getRef(_oreId);
            _m_eOreState = _oreState;
            _m_iNum = _num;
            _m_iMass = _mass;
            _m_lGetTimeMs = _getTimeMs;
            _m_normalSkillInfo = _normalSkillInfo;
            _m_advanceSkillInfo = _advanceSkillInfo;
        }

        public TreasureHuntCommonOreInfo(TreasureHuntOreRefObj _oreRefObj, ETreasureHuntOreState _oreState, int _num, int _mass, long _getTimeMs, _ITreasureHuntSkillInfo _normalSkillInfo = null, _ITreasureHuntSkillInfo _advanceSkillInfo = null)
        {
            _m_oreRefObj = _oreRefObj;
            _m_eOreState = _oreState;
            _m_iNum = _num;
            _m_iMass = _mass;
            _m_lGetTimeMs = _getTimeMs;
            _m_normalSkillInfo = _normalSkillInfo;
            _m_advanceSkillInfo = _advanceSkillInfo;
        }
        
        public long oreId { get { return _m_oreRefObj?.id ?? 0; } }
        public TreasureHuntOreRefObj oreRefObj { get { return _m_oreRefObj; } }
        public ETreasureHuntOreState oreState { get { return _m_eOreState; } }
        public int num { get { return _m_iNum; } }
        public int mass { get { return _m_iMass; } }
        public long getTimeMs { get { return _m_lGetTimeMs; } }
        public _ITreasureHuntSkillInfo normalSkillInfo { get { return _m_normalSkillInfo; } }
        public _ITreasureHuntSkillInfo advanceSkillInfo { get { return _m_advanceSkillInfo; } }

        public void updateOreState(ETreasureHuntOreState _oreState)
        {
            _m_eOreState = _oreState;
        }

        public void updateNum(int _num)
        {
            _m_iNum = _num;
        }
    }
}