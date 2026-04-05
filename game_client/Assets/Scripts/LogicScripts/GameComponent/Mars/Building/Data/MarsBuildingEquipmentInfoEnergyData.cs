using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MarsBuildingEquipmentInfoEnergyData
    {
        [NotNull] private readonly MarsBuildingEquipmentInfo _m_equipmentInfo;
        private MarsEquipmentEnergyLevelRefObj _m_refObj;
        private MarsEquipmentEnergyLevelRefObj _m_nextRefObj;
        
        
        public MarsBuildingEquipmentInfoEnergyData([NotNull] MarsBuildingEquipmentInfo _equipmentInfo)
        {
            _m_equipmentInfo = _equipmentInfo;
            _updateLevel(1);
        }
        
        
        public MarsEquipmentEnergyLevelRefObj refObj { get { return _m_refObj; } }
        
        
        public bool isValid()
        {
            return _m_refObj != null;
        }
        
        
        internal void _addProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.energyProperty._addOutputValuePerMin(_m_refObj.output_value_per_min);
            _m_equipmentInfo.buildingInfo.energyProperty._addMaxStorage(_m_refObj.max_storage);
            _m_equipmentInfo.buildingInfo.energyProperty._addPeopleOutputValuePerMin(_m_refObj.people_output_value_per_min);
        }
        internal void _removeProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.energyProperty._removeOutputValuePerMin(_m_refObj.output_value_per_min);
            _m_equipmentInfo.buildingInfo.energyProperty._removeMaxStorage(_m_refObj.max_storage);
            _m_equipmentInfo.buildingInfo.energyProperty._removePeopleOutputValuePerMin(_m_refObj.people_output_value_per_min);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_refObj != null && _m_refObj.level == _level)
                return;
            
            if (_m_equipmentInfo._propertyAdded)
                _removeProperty();
            
            _m_refObj = GRefdataCoreMgr.instance.getMarsEquipmentEnergyLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level);
            _m_nextRefObj = GRefdataCoreMgr.instance.getMarsEquipmentEnergyLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level + 1);
            
            if (_m_equipmentInfo._propertyAdded)
                _addProperty();
        }


        [NotNull]
        public List<MarsBuildingEquipmentPropertyShowData> getPropertyShowData()
        {
            if (_m_refObj == null)
                return new List<MarsBuildingEquipmentPropertyShowData>(0);
            
            List<MarsBuildingEquipmentPropertyShowData> result = new List<MarsBuildingEquipmentPropertyShowData>();
            if (_m_refObj.output_value_per_min > 0 || _m_nextRefObj is { output_value_per_min: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameEnergyOutput_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescEnergyOutput_none),
                    icon = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long)ECurrency.MARS_ENERGY),
                    currentValueStr = getOutputValuePerMinStr(_m_refObj),
                    nextValueStr = getOutputValuePerMinStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }
            if (_m_refObj.max_storage > 0 || _m_nextRefObj is { max_storage: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameEnergyMaxStorage_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescEnergyMaxStorage_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_energy_max_storage_icon,
                    currentValueStr = getMaxStorageStr(_m_refObj),
                    nextValueStr = getMaxStorageStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }

            return result;
        }
        
        
        private string getOutputValuePerMinStr(MarsEquipmentEnergyLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;
            
            return TextTranslate.instance.getLanguage(TransKeyConst.mars_commonPerMin_num, _refObj.output_value_per_min);
        }
        private string getOutputValuePerMinStrPlus(MarsEquipmentEnergyLevelRefObj _nextRefObj, MarsEquipmentEnergyLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.output_value_per_min - _refObj.output_value_per_min;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
        private string getMaxStorageStr(MarsEquipmentEnergyLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;
            
            return _refObj.max_storage.ToString();
        }
        private string getMaxStorageStrPlus(MarsEquipmentEnergyLevelRefObj _nextRefObj, MarsEquipmentEnergyLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.max_storage - _refObj.max_storage;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
    }
}