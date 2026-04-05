using Common.MarsObj;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingEquipmentInfo
    {
        [NotNull] private readonly MarsBuildingInfo _m_buildingInfo;
        [NotNull] private readonly MarsEquipmentRefObj _m_equipmentRefObj;
        private readonly bool _m_isMain;
        
        [NotNull] private readonly MarsBuildingEquipmentInfoEnergyData _m_energyData;
        [NotNull] private readonly MarsBuildingEquipmentInfoFoodData _m_foodData;
        [NotNull] private readonly MarsBuildingEquipmentInfoHospitalData _m_hospitalData;
        [NotNull] private readonly MarsBuildingEquipmentInfoLivingData _m_livingData;
        
        private int _m_level;
        private MarsEquipmentLevelRefObj _m_currentLevelRef;
        private MarsEquipmentLevelRefObj _m_nextLevelRef;

        private int _m_buildingLevel;
        private bool _m_propertyAdded;
        
        
        public MarsBuildingEquipmentInfo([NotNull] MarsBuildingInfo _buildingInfo, [NotNull] MarsEquipmentRefObj _equipmentRefObj, bool _isMain)
        {
            _m_buildingInfo = _buildingInfo;
            _m_equipmentRefObj = _equipmentRefObj;
            _m_isMain = _isMain;
            
            _m_energyData = new MarsBuildingEquipmentInfoEnergyData(this);
            _m_foodData = new MarsBuildingEquipmentInfoFoodData(this);
            _m_hospitalData = new MarsBuildingEquipmentInfoHospitalData(this);
            _m_livingData = new MarsBuildingEquipmentInfoLivingData(this);
            
            _updateLevel(1);
            _updateBuildingLevel(1);
            _m_propertyAdded = false;
        }
        
        
        [NotNull] public MarsBuildingInfo buildingInfo { get { return _m_buildingInfo; } }
        [NotNull] public MarsEquipmentRefObj refObj { get { return _m_equipmentRefObj; } }
        [NotNull] public MarsBuildingEquipmentInfoEnergyData energyData { get { return _m_energyData; } }
        [NotNull] public MarsBuildingEquipmentInfoFoodData foodData { get { return _m_foodData; } }
        [NotNull] public MarsBuildingEquipmentInfoHospitalData hospitalData { get { return _m_hospitalData; } }
        [NotNull] public MarsBuildingEquipmentInfoLivingData livingData { get { return _m_livingData; } }
        public int level { get { return _m_level; } }
        public MarsEquipmentLevelRefObj currentLevelRef { get { return _m_currentLevelRef; } }
        public MarsEquipmentLevelRefObj nextLevelRef { get { return _m_nextLevelRef; } }
        public bool isLevelMax { get { return _m_nextLevelRef == null; } }
        public bool isLevelLimit { get { return _m_level >= currentMaxLevel; } }
        public string nameTranslated { get { return TextTranslate.instance.getLanguage(_m_equipmentRefObj.name); } }
        public string descTranslated { get { return TextTranslate.instance.getLanguage(_m_equipmentRefObj.desc); } }
        public bool isUnlock { get { return _m_buildingLevel >= _m_equipmentRefObj.unlock_level; } }
        public int currentMaxLevel { get { return _m_equipmentRefObj.getMaxLevel(_m_buildingLevel); } }
        public bool isMain { get { return _m_isMain; } }
        
        internal bool _propertyAdded { get { return _m_propertyAdded; } }
        
        
        internal void _addProperty()
        {
            if (!isUnlock)
                return;
            
            _m_energyData._addProperty();
            _m_foodData._addProperty();
            _m_hospitalData._addProperty();
            _m_livingData._addProperty();
            _m_propertyAdded = true;
        }
        internal void _removeProperty()
        {
            if (!isUnlock)
                return;
            
            _m_energyData._removeProperty();
            _m_foodData._removeProperty();
            _m_hospitalData._removeProperty();
            _m_livingData._removeProperty();
            _m_propertyAdded = false;
        }
        internal void _updateLevel(int _level)
        {
            if (_m_level == _level)
                return;
            
            _m_level = _level;
            _m_currentLevelRef = GRefdataCoreMgr.instance.getMarsEquipmentLevelRef(_m_equipmentRefObj.upgrade_group_id, _m_level);
            _m_nextLevelRef = GRefdataCoreMgr.instance.getMarsEquipmentLevelRef(_m_equipmentRefObj.upgrade_group_id, _m_level + 1);
            _m_energyData._updateLevel(_level);
            _m_foodData._updateLevel(_level);
            _m_hospitalData._updateLevel(_level);
            _m_livingData._updateLevel(_level);
        }
        internal void _updateData(Mars_BuildingEquipment _serverEquipmentInfo)
        {
            if (_serverEquipmentInfo == null)
                return;
            
            _updateLevel(_serverEquipmentInfo.getLvl());
        }
        internal void _updateBuildingLevel(int _level)
        {
            if (_m_buildingLevel == _level)
                return;
            
            if (_m_buildingInfo._propertyAdded)
                _removeProperty();

            _m_buildingLevel = _level;
            
            if (_m_buildingInfo._propertyAdded)
                _addProperty();
        }


        /// <summary>
        /// 检查部件是否可以升级
        /// </summary>
        /// <param name="_checkResEnough">是否检查资源是否足够</param>
        /// <returns>是否可以升级</returns>
        public bool checkCanUpgrade(bool _checkResEnough = true)
        {
            // 部件未解锁，不能升级
            if (!isUnlock)
                return false;
            
            // 部件已达到当前最大等级或已满级，不能升级
            if (isLevelMax || isLevelLimit)
                return false;

            if (_m_currentLevelRef == null)
                return false;
            
            // 检查资源是否足够
            if (_checkResEnough)
            {
                if (!GCommon.isItemEnough(_m_currentLevelRef.upgrade_cost, false))
                    return false;
            }
            
            return true;
        }
    }
}