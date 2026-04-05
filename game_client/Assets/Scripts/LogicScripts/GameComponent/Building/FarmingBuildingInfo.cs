
using Common.BuildingObj;
using JetBrains.Annotations;

namespace GOE
{
    public class FarmingBuildingInfo
    {
        [NotNull] private readonly FarmingBuildingRefObj _m_baseRef;
        private FarmingBuildingLevelData _m_levelData;
        private FarmingBuildingLevelData _m_nextLevelData;
        private long _m_lastClientClickMS;
        private long _m_earningsPerS;
        
        
        public FarmingBuildingInfo([NotNull] Building_Farm _serverData, bool _isDealNextFrame = true)
        {
            _m_baseRef = GRefdataCoreMgr.instance.farmingBuildingRefCore.getRef(_serverData.getBuildingId());
            update(_serverData, _isDealNextFrame);
        }
        
        
        [NotNull] public FarmingBuildingRefObj baseRef { get { return _m_baseRef; } }
        public FarmingBuildingLevelData levelData { get { return _m_levelData; } }
        public FarmingBuildingLevelData nextLevelData { get { return _m_nextLevelData; } }
        public int level { get { return _m_levelData?.level ?? 1; } }
        public bool levelMax { get { return _m_nextLevelData == null; } }
        public long id { get { return _m_baseRef.building_id; } }
        public long earningsPerS { get { return _m_earningsPerS; } }
        public long lastClientClickMS { get { return _m_lastClientClickMS; } internal set { _m_lastClientClickMS = value; } }
        public long businessBuildingBonus { get { return _m_levelData?.earning_rate ?? 0; } }
        public long clickEarnings { get { return _m_levelData?.tap_to_collect_num ?? 0; } }


        public void update(Building_Farm _serverData, bool _isDealNextFrame = true)
        {
            if (_serverData == null)
                return;
            
            FarmingBuildingLevelRefObj currentLevelPhaseRef = GRefdataCoreMgr.instance.getFarmingBuildingLevelRefObj(_serverData.getBuildingId(), _serverData.getLvl());
            FarmingBuildingLevelRefObj nextLevelPhaseRef = GRefdataCoreMgr.instance.getFarmingBuildingLevelRefObj(_serverData.getBuildingId(), _serverData.getLvl() + 1);
            if (currentLevelPhaseRef == null)
            {
                _m_levelData = null;
                _m_earningsPerS = 0;
            }
            else
            {
                _m_levelData = new FarmingBuildingLevelData(_m_baseRef, currentLevelPhaseRef, _serverData.getLvl());
                _m_earningsPerS = _m_levelData.auto_tap_num_per_sec * _m_levelData.tap_to_collect_num;
            }
            _m_nextLevelData = nextLevelPhaseRef == null ? null : new FarmingBuildingLevelData(_m_baseRef, nextLevelPhaseRef, _serverData.getLvl() + 1);

            NPPlayer.instance.buildingComp.recalAllBusinessBuilding(_isDealNextFrame);
        }
        public NPGGoIndex getCurrentResIndex()
        {
            NPGGoIndex resIndex = _m_levelData?.res_index;
            if (resIndex == null || !resIndex.isValid())
                return _m_baseRef.res_index;
            
            return resIndex;
        }
        public NPGTextureIndex getCurrentPreviewTexIndex()
        {
            return _getPreviewTexIndex(_m_levelData);
        }
        public NPGTextureIndex getNextPreviewTexIndex()
        {
            return _getPreviewTexIndex(_m_nextLevelData ?? _m_levelData);
        }
        
        
        private NPGTextureIndex _getPreviewTexIndex(FarmingBuildingLevelData _levelData)
        {
            NPGTextureIndex index = _levelData?.preview_tex_index;
            if (index == null || !index.isValid())
                return _m_baseRef.preview_tex_index;

            return index;
        }
    }
}