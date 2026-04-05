using System.Collections.Generic;
using ALPackage;
using Common.MarsObj;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoEquipmentData
    {
        [NotNull] private readonly MarsBuildingInfo _m_buildingInfo;
        [NotNull, ItemNotNull] private readonly List<MarsBuildingEquipmentInfo> _m_mainEquipmentInfos;
        [NotNull, ItemNotNull] private readonly List<MarsBuildingEquipmentInfo> _m_otherEquipmentInfos;
        [NotNull, ItemNotNull] private readonly List<MarsBuildingEquipmentInfo> _m_allEquipmentInfos;
        private int _m_level;


        public MarsBuildingInfoEquipmentData([NotNull] MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _m_mainEquipmentInfos = new List<MarsBuildingEquipmentInfo>();
            _m_otherEquipmentInfos = new List<MarsBuildingEquipmentInfo>();
            _m_allEquipmentInfos = new List<MarsBuildingEquipmentInfo>();
            
            MarsBuildingEquipmentBelongRefObj belongRefObj = GRefdataCoreMgr.instance.getMarsBuildingEquipmentBelongRef(_m_buildingInfo.refObj.upgrade_group_id);
            if (belongRefObj != null)
            {
                if (belongRefObj.main_equipment_id_list != null)
                {
                    foreach (long equipmentId in belongRefObj.main_equipment_id_list)
                    {
                        MarsEquipmentRefObj equipmentRefObj = GRefdataCoreMgr.instance.marsEquipmentRefCore.getRef(equipmentId);
                        if (equipmentRefObj != null)
                        {
                            MarsBuildingEquipmentInfo equipmentInfo = new MarsBuildingEquipmentInfo(_m_buildingInfo, equipmentRefObj, true);
                            _m_mainEquipmentInfos.Add(equipmentInfo);
                            _m_allEquipmentInfos.Add(equipmentInfo);
                        }
                    }
                }
                if (belongRefObj.other_equipment_id_list != null)
                {
                    foreach (long equipmentId in belongRefObj.other_equipment_id_list)
                    {
                        MarsEquipmentRefObj equipmentRefObj = GRefdataCoreMgr.instance.marsEquipmentRefCore.getRef(equipmentId);
                        if (equipmentRefObj != null)
                        {
                            MarsBuildingEquipmentInfo equipmentInfo = new MarsBuildingEquipmentInfo(_m_buildingInfo, equipmentRefObj, false);
                            _m_otherEquipmentInfos.Add(equipmentInfo);
                            _m_allEquipmentInfos.Add(equipmentInfo);
                        }
                    }
                }
            }
            
            _updateLevel(1);
        }


        public int totalMainLevelStart
        {
            get
            {
                int result = 0;
                int buildingLevel = _m_buildingInfo.level - 1;
                foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_mainEquipmentInfos)
                {
                    if (equipmentInfo.refObj.unlock_level == buildingLevel + 1)
                        result += 1;
                    else if (buildingLevel >= equipmentInfo.refObj.unlock_level)
                        result += equipmentInfo.refObj.getMaxLevel(buildingLevel);
                }

                return result;
            }
        }
        public int totalMainLevelEnd
        {
            get
            {
                int result = 0;
                foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_mainEquipmentInfos)
                {
                    if (!equipmentInfo.isUnlock)
                        continue;
                    
                    result += equipmentInfo.currentMaxLevel;
                }

                return result;
            }
        }
        public int totalMainLevel
        {
            get
            {
                int result = 0;
                foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_mainEquipmentInfos)
                {
                    if (!equipmentInfo.isUnlock)
                        continue;
                    
                    result += equipmentInfo.level;
                }

                return result;
            }
        }
        
        /// <summary>
        /// 部件等级进度是否完成
        /// </summary>
        public bool equipmentLevelProgressIsComplete 
        {
            get
            {
                // 若无部件信息，则视为完成
                if (!isValid())
                    return true;
                
                return totalMainLevel >= totalMainLevelEnd;
            }
        }
        
        public List<MarsBuildingEquipmentInfo> equipmentList { get { return _m_allEquipmentInfos; } }


        public bool isValid()
        {
            return _m_allEquipmentInfos.Count > 0;
        }
        public MarsBuildingEquipmentInfo getEquipmentById(long _equipmentId)
        {
            foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_allEquipmentInfos)
            {
                if (equipmentInfo.refObj.id == _equipmentId)
                    return equipmentInfo;
            }

            return null;
        }
        public int getTotalLevel()
        {
            int result = 0;
            foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_allEquipmentInfos)
            {
                if (!equipmentInfo.isUnlock)
                    continue;
                
                result += equipmentInfo.level;
            }

            return result;
        }
        
        
        internal void _addProperty()
        {
            foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_allEquipmentInfos)
                equipmentInfo._addProperty();
        }
        internal void _removeProperty()
        {
            foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_allEquipmentInfos)
                equipmentInfo._removeProperty();
        }
        internal void _updateData(Mars_BuildingEquipment _serverEquipmentInfo)
        {
            if (_serverEquipmentInfo == null)
                return;

            MarsBuildingEquipmentInfo equipmentInfo = _m_allEquipmentInfos.Find(_info => _info.refObj.id == _serverEquipmentInfo.getEquipmentId());
            if (equipmentInfo == null)
            {
                ALLog.Error($"[MarsBuildingInfoEquipmentData] _updateData: 未找到对应的建筑部件信息, buildingId={_serverEquipmentInfo.getBuildingId()}, equipmentId={_serverEquipmentInfo.getEquipmentId()}");
                return;
            }
            
            equipmentInfo._updateData(_serverEquipmentInfo);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_level == _level)
                return;
            
            _m_level = _level;
            foreach (MarsBuildingEquipmentInfo equipmentInfo in _m_allEquipmentInfos)
                equipmentInfo._updateBuildingLevel(_level);
        }
    }
}