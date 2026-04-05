using Common.DungeonObj;

namespace GOE
{
    public class MiddayDungeonBoxInfo
    {
        private string _m_playerName;
        private long _m_playerCid;
        private long _m_dbId;
        private long _m_expireTs;
        private long _m_boxId;
        
        private MiddayDungeonBoxRefObj _m_boxRefObj;
        
        public MiddayDungeonBoxInfo(MiddayDungeon_BoxInfo _boxInfo)
        {
            _m_dbId = _boxInfo.getDbId();
            _m_playerName = _boxInfo.getSenderName();
            _m_playerCid = _boxInfo.getSenderCid();
            _m_expireTs = _boxInfo.getExpireTimeMS();
            _m_boxId = _boxInfo.getBoxId();
            _m_boxRefObj = GRefdataCoreMgr.instance.middayDungeonBoxRefCore.getRef(_m_boxId);
        }

        public long playerCid => _m_playerCid;
        public string playerName => _m_playerName;
        public long dbId => _m_dbId;
        public long expireTs => _m_expireTs;
        public bool isValid => _m_expireTs - FpsAndPingMgr.instance.serverTimeTagS > 0;
        public long boxId => _m_boxId;
        public MiddayDungeonBoxRefObj boxRefObj => _m_boxRefObj;
    }
}