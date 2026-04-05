using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MarsBuildingEquipmentInfoHospitalData
    {
        [NotNull] private readonly MarsBuildingEquipmentInfo _m_equipmentInfo;
        private MarsEquipmentHospitalLevelRefObj _m_refObj;
        private MarsEquipmentHospitalLevelRefObj _m_nextRefObj;
        
        
        public MarsBuildingEquipmentInfoHospitalData([NotNull] MarsBuildingEquipmentInfo _equipmentInfo)
        {
            _m_equipmentInfo = _equipmentInfo;
            _updateLevel(1);
        }
        
        
        public MarsEquipmentHospitalLevelRefObj refObj { get { return _m_refObj; } }
        
        
        public bool isValid()
        {
            return _m_refObj != null;
        }
        
        
        internal void _addProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.cureRateProperty._addCureRate(_m_refObj.cure_rate);
        }
        internal void _removeProperty()
        {
            if (_m_refObj == null)
                return;
                
            _m_equipmentInfo.buildingInfo.cureRateProperty._minusCureRate(_m_refObj.cure_rate);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_refObj != null && _m_refObj.level == _level)
                return;
            
            if (_m_equipmentInfo._propertyAdded)
                _removeProperty();
            
            _m_refObj = GRefdataCoreMgr.instance.getMarsEquipmentHospitalLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level);
            _m_nextRefObj = GRefdataCoreMgr.instance.getMarsEquipmentHospitalLevelRef(_m_equipmentInfo.refObj.upgrade_group_id, _level + 1);
            
            if (_m_equipmentInfo._propertyAdded)
                _addProperty();
        }


        [NotNull]
        public List<MarsBuildingEquipmentPropertyShowData> getPropertyShowData()
        {
            if (_m_refObj == null)
                return new List<MarsBuildingEquipmentPropertyShowData>(0);
            
            List<MarsBuildingEquipmentPropertyShowData> result = new List<MarsBuildingEquipmentPropertyShowData>();
            if (_m_refObj.cure_rate > 0 || _m_nextRefObj is { cure_rate: > 0 })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameCureRate_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescCureRate_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_hospital_cure_rate_icon,
                    currentValueStr = getCureRateStr(_m_refObj),
                    nextValueStr = getCureRateStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }

            if (_m_refObj.cure_num_range.max > 0 || _m_nextRefObj is { cure_num_range: { max: > 0 } })
            {
                result.Add(new MarsBuildingEquipmentPropertyShowData()
                {
                    propertyName = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyNameCureNumRange_none),
                    propertyDesc = TextTranslate.instance.getLanguage(TransKeyConst.mars_propertyDescCureNumRange_none),
                    icon = GRefdataCoreMgr.instance.npGeneral.mars_equipment_hospital_cure_num_range_icon,
                    currentValueStr = getCureNumRangeStr(_m_refObj),
                    nextValueStr = getCureNumRangeStrPlus(_m_nextRefObj, _m_refObj),
                    isLevelMax = _m_nextRefObj == null,
                });
            }

            return result;
        }
        
        
        private string getCureRateStr(MarsEquipmentHospitalLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _refObj.cure_rate / 100f);
        }
        private string getCureRateStrPlus(MarsEquipmentHospitalLevelRefObj _nextRefObj, MarsEquipmentHospitalLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.cure_rate - _refObj.cure_rate;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, -plusValue / 100f))
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, plusValue / 100f));
        }
        private string getCureNumRangeStr(MarsEquipmentHospitalLevelRefObj _refObj)
        {
            if (_refObj == null)
                return string.Empty;

            return _refObj.cure_num_range.max.ToString();
        }
        private string getCureNumRangeStrPlus(MarsEquipmentHospitalLevelRefObj _nextRefObj, MarsEquipmentHospitalLevelRefObj _refObj)
        {
            if (_nextRefObj == null || _refObj == null)
                return string.Empty;

            long plusValue = _nextRefObj.cure_num_range.max - _refObj.cure_num_range.max;
            return plusValue < 0
                ? TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, -plusValue)
                : TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, plusValue);
        }
    }
}