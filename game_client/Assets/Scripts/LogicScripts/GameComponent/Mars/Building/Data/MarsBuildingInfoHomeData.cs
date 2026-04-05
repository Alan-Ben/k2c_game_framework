using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoHomeData
    {
        [NotNull] private readonly MarsBuildingInfo _m_buildingInfo;
        private MarsBuildingHomeLevelRefObj _m_refObj;
        private MarsBuildingHomeLevelRefObj _m_nextRefObj;


        public MarsBuildingInfoHomeData([NotNull] MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _updateLevel(1);
        }


        public MarsBuildingHomeLevelRefObj refObj { get { return _m_refObj; } }
        public MarsBuildingHomeLevelRefObj nextRefObj { get { return _m_nextRefObj; } }
        
        
        public bool isValid()
        {
            return _m_refObj != null;
        }
        
        
        internal void _addProperty()
        {
            if (_m_refObj == null)
                return;

            _m_buildingInfo.oxygenYieldProperty._addOffOxygenYield(_m_refObj.off_oxygen_yield);
            _m_buildingInfo.oxygenYieldProperty._addOnOxygenYield(_m_refObj.on_oxygen_yield);
            _m_buildingInfo.oxygenYieldProperty._addOverdriveOxygenYield(_m_refObj.overdrive_oxygen_yield);
            _m_buildingInfo.oxygenYieldProperty._addEnergyConsumePerMin(_m_refObj.energy_consume_per_min);
            _m_buildingInfo.oxygenYieldProperty._addOverdriveEnergyConsumePerMin(_m_refObj.overdrive_energy_consume_per_min);
            _m_buildingInfo.oxygenYieldProperty._addOxygenYieldAdjustCoefficient(_m_refObj.oxygen_yield_adjust_coefficient);
        }
        internal void _removeProperty()
        {
            if (_m_refObj == null)
                return;

            _m_buildingInfo.oxygenYieldProperty._minusOffOxygenYield(_m_refObj.off_oxygen_yield);
            _m_buildingInfo.oxygenYieldProperty._minusOnOxygenYield(_m_refObj.on_oxygen_yield);
            _m_buildingInfo.oxygenYieldProperty._minusOverdriveOxygenYield(_m_refObj.overdrive_oxygen_yield);
            _m_buildingInfo.oxygenYieldProperty._minusEnergyConsumePerMin(_m_refObj.energy_consume_per_min);
            _m_buildingInfo.oxygenYieldProperty._minusOverdriveEnergyConsumePerMin(_m_refObj.overdrive_energy_consume_per_min);
            _m_buildingInfo.oxygenYieldProperty._minusOxygenYieldAdjustCoefficient(_m_refObj.oxygen_yield_adjust_coefficient);
        }
        internal void _updateLevel(int _level)
        {
            if (_m_refObj != null && _m_refObj.level == _level)
                return;
            
            if (_m_buildingInfo._propertyAdded)
                _removeProperty();

            if (_m_buildingInfo.refObj.id == GRefdataCoreMgr.instance.npGeneral.mars_building_home_id)
            {
                _m_refObj = GRefdataCoreMgr.instance.getMarsBuildingHomeLevelRef(_level);
                _m_nextRefObj = GRefdataCoreMgr.instance.getMarsBuildingHomeLevelRef(_level + 1);
            }
            
            if (_m_buildingInfo._propertyAdded)
                _addProperty();
        }
    }
}