using Common.TreasureHuntObj;

namespace GOE
{
    /// <summary>
    /// 太空舱数据
    /// </summary>
    public class TreasureHuntStationInfo
    {
        private int _m_iStationLevel;//太空舱等级
        private TreasureHuntStationLvlRefObj _m_stationLvlRefObj;//太空舱等级配表对象
        private long _m_lStationExp;//太空舱经验

        public TreasureHuntStationInfo(TreasureHunt_StationInfo _stationInfo)
        {
            updateInfo(_stationInfo);
        }

        public int stationLevel { get { return _m_iStationLevel; } }
        public TreasureHuntStationLvlRefObj stationLvlRefObj
        {
            get
            {
                if (_m_stationLvlRefObj == null || _m_stationLvlRefObj.level != _m_iStationLevel)
                    _m_stationLvlRefObj = GRefdataCoreMgr.instance.treasureHuntStationLvlRefCore.getRef(_m_iStationLevel);
                return _m_stationLvlRefObj;
            }
        }
        public long stationExp { get { return _m_lStationExp; } }

        public void updateInfo(TreasureHunt_StationInfo _stationInfo)
        {
            if(_stationInfo == null)
                return;
            
            _m_iStationLevel = _stationInfo.getStationLevel();
            _m_lStationExp = _stationInfo.getExp();
        }
    }
}