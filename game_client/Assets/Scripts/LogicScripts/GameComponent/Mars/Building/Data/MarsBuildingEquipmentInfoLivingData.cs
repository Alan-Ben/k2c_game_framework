using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingEquipmentInfoLivingData
    {
        [NotNull] private readonly MarsBuildingEquipmentInfo _m_equipmentInfo;
        private MarsEquipmentLivingLevelRefObj _m_refObj;
        private MarsEquipmentLivingLevelRefObj _m_nextRefObj;
        
        
        public MarsBuildingEquipmentInfoLivingData([NotNull] MarsBuildingEquipmentInfo _equipmentInfo)
        {
            _m_equipmentInfo = _equipmentInfo;
            _updateLevel(1);
        }
        
        
        public MarsEquipmentLivingLevelRefObj refObj { get { return _m_refObj; } }
        
        
        public bool isValid()
        {
            return _m_refObj != null;
        }
        
        
        internal void _addProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.comfortYieldProperty._addComfortYield(_m_refObj.comfort_yield);
            _m_equipmentInfo.buildingInfo.moodYieldProperty._addMoodYield(_m_refObj.mood_yield);
            _m_equipmentInfo.buildingInfo.sleepYieldProperty._addSleepYield(_m_refObj.sleep_yield);
            _m_equipmentInfo.buildingInfo.peopleNumLimitProperty._addPeopleNumLimit(_m_refObj.people_num_limit);
        }
        internal void _removeProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.comfortYieldProperty._minusComfortYield(_m_refObj.comfort_yield);
            _m_equipmentInfo.buildingInfo.moodYieldProperty._minusMoodYield(_m_refObj.mood_yield);
            _m_equipmentInfo.buildingInfo.sleepYieldProperty._minusSleepYield(_m_refObj.sleep_yield);
            _m_equipmentInfo.buildingInfo.peopleNumLimitProperty._minusPeopleNumLimit(_m_refObj.people_num_limit);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_refObj != null && _m_refObj.level == _level)
                return;
            
            if (_m_equipmentInfo._propertyAdded)
                _removeProperty();
            
            _m_refObj = GRefdataCoreMgr.instance.getMarsEquipmentLivingLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level);
            _m_nextRefObj = GRefdataCoreMgr.instance.getMarsEquipmentLivingLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level + 1);
            
            if (_m_equipmentInfo._propertyAdded)
                _addProperty();
        }


        [NotNull]
        public List<MarsBuildingEquipmentPropertyShowData> getPropertyShowData()
        {
            if (_m_refObj == null)
                return new List<MarsBuildingEquipmentPropertyShowData>(0);
            
            List<MarsBuildingEquipmentPropertyShowData> result = new List<MarsBuildingEquipmentPropertyShowData>();
            if (_m_refObj.comfort_yield > 0 || _m_nextRefObj is { comfort_yield: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameComfortYield_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescComfortYield_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_living_comfort_yield_icon,
                    currentValueStr = getComfortYieldStr(_m_refObj),
                    nextValueStr = getComfortYieldStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }
            if (_m_refObj.mood_yield > 0 || _m_nextRefObj is { mood_yield: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameMoodYield_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescMoodYield_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_living_mood_yield_icon,
                    currentValueStr = getMoodYieldStr(_m_refObj),
                    nextValueStr = getMoodYieldStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }
            if (_m_refObj.sleep_yield > 0 || _m_nextRefObj is { sleep_yield: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameSleepYield_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescSleepYield_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_living_sleep_yield_icon,
                    currentValueStr = getSleepYieldStr(_m_refObj),
                    nextValueStr = getSleepYieldStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }
            if (_m_refObj.people_num_limit > 0 || _m_nextRefObj is { people_num_limit: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameLivingPeopleNumLimit_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescLivingPeopleNumLimit_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_living_people_num_limit_icon,
                    currentValueStr = getPeopleNumLimitStr(_m_refObj),
                    nextValueStr = getPeopleNumLimitStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }

            return result;
        }
        
        
        private string getComfortYieldStr(MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return _refObj.comfort_yield.ToString();
        }
        private string getComfortYieldStrPlus(MarsEquipmentLivingLevelRefObj _nextRefObj, MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.comfort_yield - _refObj.comfort_yield;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
        private string getMoodYieldStr(MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return _refObj.mood_yield.ToString();
        }
        private string getMoodYieldStrPlus(MarsEquipmentLivingLevelRefObj _nextRefObj, MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.mood_yield - _refObj.mood_yield;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
        private string getSleepYieldStr(MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return _refObj.sleep_yield.ToString();
        }
        private string getSleepYieldStrPlus(MarsEquipmentLivingLevelRefObj _nextRefObj, MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.sleep_yield - _refObj.sleep_yield;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
        private string getPeopleNumLimitStr(MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return _refObj.people_num_limit.ToString();
        }
        private string getPeopleNumLimitStrPlus(MarsEquipmentLivingLevelRefObj _nextRefObj, MarsEquipmentLivingLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.people_num_limit - _refObj.people_num_limit;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
    }
}