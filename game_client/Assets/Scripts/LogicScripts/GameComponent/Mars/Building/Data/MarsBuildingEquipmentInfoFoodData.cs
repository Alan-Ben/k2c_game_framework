using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MarsBuildingEquipmentInfoFoodData
    {
        [NotNull] private readonly MarsBuildingEquipmentInfo _m_equipmentInfo;
        private MarsEquipmentFoodLevelRefObj _m_refObj;
        private MarsEquipmentFoodLevelRefObj _m_nextRefObj;
        
        
        public MarsBuildingEquipmentInfoFoodData([NotNull] MarsBuildingEquipmentInfo _equipmentInfo)
        {
            _m_equipmentInfo = _equipmentInfo;
            _updateLevel(1);
        }
        
        
        public MarsEquipmentFoodLevelRefObj refObj { get { return _m_refObj; } }
        
        
        public bool isValid()
        {
            return _m_refObj != null;
        }
        
        
        internal void _addProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.satietyYieldProperty._addConsumePerMin(_m_refObj.energy_consume_per_min);
            _m_equipmentInfo.buildingInfo.satietyYieldProperty._addSatietyYield(_m_refObj.satiety_yield);
        }
        internal void _removeProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.satietyYieldProperty._minusConsumePerMin(_m_refObj.energy_consume_per_min);
            _m_equipmentInfo.buildingInfo.satietyYieldProperty._minusSatietyYield(_m_refObj.satiety_yield);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_refObj != null && _m_refObj.level == _level)
                return;
            
            if (_m_equipmentInfo._propertyAdded)
                _removeProperty();
            
            _m_refObj = GRefdataCoreMgr.instance.getMarsEquipmentFoodLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level);
            _m_nextRefObj = GRefdataCoreMgr.instance.getMarsEquipmentFoodLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level + 1);
            
            if (_m_equipmentInfo._propertyAdded)
                _addProperty();
        }


        [NotNull]
        public List<MarsBuildingEquipmentPropertyShowData> getPropertyShowData()
        {
            if (_m_refObj == null)
                return new List<MarsBuildingEquipmentPropertyShowData>(0);
            
            List<MarsBuildingEquipmentPropertyShowData> result = new List<MarsBuildingEquipmentPropertyShowData>();
            if (_m_refObj.energy_consume_per_min > 0 || _m_nextRefObj is { energy_consume_per_min: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameEnergyConsume_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescEnergyConsume_none),
                    icon = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long)ECurrency.MARS_ENERGY),
                    currentValueStr = getEnergyConsumePerMinStr(_m_refObj),
                    nextValueStr = getEnergyConsumePerMinStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }
            if (_m_refObj.satiety_yield > 0 || _m_nextRefObj is { satiety_yield: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameSatietyYield_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescSatietyYield_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_food_satiety_yield_icon,
                    currentValueStr = getSatietyYieldStr(_m_refObj),
                    nextValueStr = getSatietyYieldStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }

            return result;
        }
        
        
        private string getEnergyConsumePerMinStr(MarsEquipmentFoodLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return TextTranslate.instance.getLanguage(TransKeyConst.mars_commonPerMin_num, _refObj.energy_consume_per_min);
        }
        private string getEnergyConsumePerMinStrPlus(MarsEquipmentFoodLevelRefObj _nextRefObj, MarsEquipmentFoodLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.energy_consume_per_min - _refObj.energy_consume_per_min;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
        private string getSatietyYieldStr(MarsEquipmentFoodLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return _refObj.satiety_yield.ToString();
        }
        private string getSatietyYieldStrPlus(MarsEquipmentFoodLevelRefObj _nextRefObj, MarsEquipmentFoodLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.satiety_yield - _refObj.satiety_yield;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
    }
}