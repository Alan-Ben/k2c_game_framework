using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MarsBuildingInfoLevelData
    {
        [NotNull] private readonly MarsBuildingInfo _m_buildingInfo;
        
        private MarsBuildingLevelRefObj _m_currentLevelRefObj;
        private MarsBuildingLevelRefObj _m_nextLevelRefObj;
        

        public MarsBuildingInfoLevelData([NotNull] MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _updateLevel(1);
        }
        
        
        public int currentLevel { get { return _m_currentLevelRefObj?.level ?? 1; } }
        public bool isLevelMax { get { return _m_nextLevelRefObj == null; } }
        public long currentPower { get { return _m_currentLevelRefObj?.mars_power_value ?? 0; } }
        public long nextPower { get { return _m_nextLevelRefObj?.mars_power_value ?? currentPower; } }
        public MarsBuildingLevelRefObj refObj { get { return _m_currentLevelRefObj; } }
        public MarsBuildingLevelRefObj nextRefObj { get { return _m_nextLevelRefObj; } }
        

        public bool isValid()
        {
            return _m_currentLevelRefObj != null;
        }
        public bool checkCanUpgrade(bool _checkResEnough = true)
        {
            if (_m_currentLevelRefObj == null || _m_nextLevelRefObj == null)
                return false;
            
            if (!MarsUtil.checkConditionIdListEnable(_m_currentLevelRefObj.upgrade_condition_id_list))
                return false;

            if (_checkResEnough)
            {
                if (!GCommon.isItemEnough(getUpgradeCostList(), false))
                    return false;
            }

            return true;
        }
        public NPGGoIndex getCurrentResIndex()
        {
            return _m_currentLevelRefObj?.upgraded_res_index;
        }
        public NPGGoIndex getNextResIndex()
        {
            return _m_nextLevelRefObj?.upgraded_res_index;
        }
        public NPGGoIndex getUpgradingResIndex()
        {
            return _m_nextLevelRefObj?.upgrading_res_index;
        }
        public long realUpgradeTimeMS()
        {
            if (refObj == null)
                return 0;
            
            long value = refObj.upgrade_time_cost_sec * 1000;
            long property = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_TIME_PER);
            value = value * 10000 / (10000 + property);
            return value;
        }
        public List<NPCommonCostItem> getUpgradeCostList()
        {
            if (_m_currentLevelRefObj?.upgrade_cost_list == null)
                return new List<NPCommonCostItem>(0);
            
            List<NPCommonCostItem> resultList = new List<NPCommonCostItem>(_m_currentLevelRefObj.upgrade_cost_list.Count);
            long costProperty = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_COST_PER);
            foreach (NPCommonCostItem costItem in _m_currentLevelRefObj.upgrade_cost_list)
            {
                if (costItem == null)
                    continue;
                
                long newCount = costItem.count * (10000 - costProperty) / 10000;
                if (newCount < 1)
                    newCount = 1;
                
                resultList.Add(new NPCommonCostItem(costItem.item, newCount));
            }

            return resultList;
        }
        

        internal void _addProperty()
        {
            if (_m_currentLevelRefObj == null)
                return;

            _m_buildingInfo.powerProperty.addValue(_m_currentLevelRefObj.mars_power_value);
            _m_buildingInfo.component.parentComponent.playerPropertyContainer.addModifier(_m_currentLevelRefObj.player_property);
            _m_buildingInfo.component.marsPropertyContainer.addModifier(_m_currentLevelRefObj.mars_property);
            _m_buildingInfo.satietyYieldProperty._addSatietyYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
            _m_buildingInfo.sleepYieldProperty._addSleepYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
            _m_buildingInfo.comfortYieldProperty._addComfortYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
            _m_buildingInfo.moodYieldProperty._addMoodYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
        }
        internal void _removeProperty()
        {
            if (_m_currentLevelRefObj == null)
                return;
            
            _m_buildingInfo.powerProperty.removeValue(_m_currentLevelRefObj.mars_power_value);
            _m_buildingInfo.component.parentComponent.playerPropertyContainer.removeModifier(_m_currentLevelRefObj.player_property);
            _m_buildingInfo.component.marsPropertyContainer.removeModifier(_m_currentLevelRefObj.mars_property);
            _m_buildingInfo.satietyYieldProperty._minusSatietyYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
            _m_buildingInfo.sleepYieldProperty._minusSleepYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
            _m_buildingInfo.comfortYieldProperty._minusComfortYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
            _m_buildingInfo.moodYieldProperty._minusMoodYieldAdjustCoefficient(_m_currentLevelRefObj.building_level_cofficient);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_currentLevelRefObj != null && _m_currentLevelRefObj.level == _level)
                return;
            
            if (_m_buildingInfo._propertyAdded)
                _removeProperty();
                
            _m_currentLevelRefObj = GRefdataCoreMgr.instance.getMarsBuildingLevelRef(_m_buildingInfo.refObj.upgrade_group_id, _level);
            if (_m_currentLevelRefObj != null)
                _m_nextLevelRefObj = GRefdataCoreMgr.instance.getMarsBuildingLevelRef(_m_buildingInfo.refObj.upgrade_group_id, _level + 1);
            else
                _m_nextLevelRefObj = null;
            
            if (_m_buildingInfo._propertyAdded)
                _addProperty();
        }
    }
}