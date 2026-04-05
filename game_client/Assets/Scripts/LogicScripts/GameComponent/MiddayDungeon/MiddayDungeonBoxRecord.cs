namespace GOE
{
    public class MiddayDungeonBoxRecord
    {
        private long _m_drawBoxTimeMs;
        private long _m_drawPlayerCid;
        private NPCommonCostItem _m_drawRewardItem;
        public MiddayDungeonBoxRecord(long _drawBoxTime, long _drawPlayerCid, NPCommonCostItem _drawRewardItem)
        {
            _m_drawBoxTimeMs = _drawBoxTime;
            _m_drawPlayerCid = _drawPlayerCid;
            _m_drawRewardItem = _drawRewardItem;
        }
        public long drawBoxTimeMs => _m_drawBoxTimeMs;
        public long drawPlayerCid => _m_drawPlayerCid;
        public NPCommonCostItem drawRewardItem => _m_drawRewardItem;
    }
}