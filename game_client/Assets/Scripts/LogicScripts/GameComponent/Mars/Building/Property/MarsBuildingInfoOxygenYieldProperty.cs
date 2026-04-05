using Common.MarsObj;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoOxygenYieldProperty : _AMarsBuildingInfoProperty
    {
        private bool _m_isOn;
        private bool _m_isOverdrive;
        private long _m_offOxygenYield;
        private long _m_onOxygenYield;
        private long _m_overdriveOxygenYield;
        private long _m_energyConsumePerMin;
        private long _m_overdriveEnergyConsumePerMin;
        private long _m_oxygenYieldAdjustCoefficient;


        public MarsBuildingInfoOxygenYieldProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, false)
        {
            _m_energyConsumePerMin = 0;
            _m_isOn = false;
            _m_isOverdrive = false;
        }
        

        public bool isOn { get { return _m_isOn; } }
        public bool isOverdrive { get { return _m_isOverdrive; } }
        public long energyConsumePerMin
        {
            get
            {
                if (_m_isOn)
                    return _m_isOverdrive ? _m_overdriveEnergyConsumePerMin : _m_energyConsumePerMin;
                
                return 0;
            }
        }
        public long valueWithoutAdjustCoefficient
        {
            get
            {
                long currentOxygenYield = _m_offOxygenYield;
                if (_m_isOn)
                    currentOxygenYield = _m_isOverdrive ? _m_overdriveOxygenYield : _m_onOxygenYield;

                return currentOxygenYield;
            }
        }


        private protected override long _computeValue()
        {
            long currentOxygenYield = _m_offOxygenYield;
            if (_m_isOn)
                currentOxygenYield = _m_isOverdrive ? _m_overdriveOxygenYield : _m_onOxygenYield;

            return currentOxygenYield * _m_oxygenYieldAdjustCoefficient;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateOxygenValue();
        }
        
        
        internal void _setSwitches(bool _isOn, bool _isOverdrive)
        {
            if (_m_isOn != _isOn || _m_isOverdrive != _isOverdrive)
            {
                _m_isOn = _isOn;
                _m_isOverdrive = _isOverdrive;
                _setDirty();
            }
        }
        internal void _addOffOxygenYield(long _oxygenYield)
        {
            _m_offOxygenYield += _oxygenYield;
            _setDirty();
        }
        internal void _addOnOxygenYield(long _oxygenYield)
        {
            _m_onOxygenYield += _oxygenYield;
            _setDirty();
        }
        internal void _addOverdriveOxygenYield(long _oxygenYield)
        {
            _m_overdriveOxygenYield += _oxygenYield;
            _setDirty();
        }
        internal void _addEnergyConsumePerMin(long _energyConsumePerMin)
        {
            _m_energyConsumePerMin += _energyConsumePerMin;
        }
        internal void _addOverdriveEnergyConsumePerMin(long _energyConsumePerMin)
        {
            _m_overdriveEnergyConsumePerMin += _energyConsumePerMin;
        }
        internal void _addOxygenYieldAdjustCoefficient(long _oxygenYieldAdjustCoefficient)
        {
            _m_oxygenYieldAdjustCoefficient += _oxygenYieldAdjustCoefficient;
            _setDirty();
        }
        internal void _minusOffOxygenYield(long _oxygenYield)
        {
            _m_offOxygenYield -= _oxygenYield;
            _setDirty();
        }
        internal void _minusOnOxygenYield(long _oxygenYield)
        {
            _m_onOxygenYield -= _oxygenYield;
            _setDirty();
        }
        internal void _minusOverdriveOxygenYield(long _oxygenYield)
        {
            _m_overdriveOxygenYield -= _oxygenYield;
            _setDirty();
        }
        internal void _minusEnergyConsumePerMin(long _energyConsumePerMin)
        {
            _m_energyConsumePerMin -= _energyConsumePerMin;
        }
        internal void _minusOverdriveEnergyConsumePerMin(long _energyConsumePerMin)
        {
            _m_overdriveEnergyConsumePerMin -= _energyConsumePerMin;
        }
        internal void _minusOxygenYieldAdjustCoefficient(long _oxygenYieldAdjustCoefficient)
        {
            _m_oxygenYieldAdjustCoefficient -= _oxygenYieldAdjustCoefficient;
            _setDirty();
        }
    }
}
