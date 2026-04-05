using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoSatietyYieldProperty : _AMarsBuildingInfoProperty
    {
        private bool _m_switchOn;
        private long _m_consumePerMin;
        private long _m_satietyYield;
        private long _m_satietyYieldAdjustCoefficient;
        

        public MarsBuildingInfoSatietyYieldProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, false)
        {
            _m_switchOn = false;
            _m_consumePerMin = 0;
            _m_satietyYield = 0;
        }
        

        public bool switchOn { get { return _m_switchOn; } }
        public long consumePerMin { get { return _m_consumePerMin; } }
        public long satietyYield { get { return _m_satietyYield; } }
        

        private protected override long _computeValue()
        {
            long yieldValue = !_m_switchOn ? 0 : _m_satietyYield;
            return yieldValue * _m_satietyYieldAdjustCoefficient;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateSatietyValue();
        }
        
        
        internal void _setSwitchOn(bool _switchOn)
        {
            if (_m_switchOn != _switchOn)
            {
                _m_switchOn = _switchOn;
                _setDirty();
            }
        }
        internal void _addConsumePerMin(long _refObjEnergyConsumePerMin)
        {
            _m_consumePerMin += _refObjEnergyConsumePerMin;
        }
        internal void _addSatietyYield(long _refObjSatietyYield)
        {
            _m_satietyYield += _refObjSatietyYield;
            _setDirty();
        }
        internal void _addSatietyYieldAdjustCoefficient(long _value)
        {
            _m_satietyYieldAdjustCoefficient += _value;
            _setDirty();
        }
        internal void _minusConsumePerMin(long _refObjEnergyConsumePerMin)
        {
            _m_consumePerMin -= _refObjEnergyConsumePerMin;
        }
        internal void _minusSatietyYield(long _refObjSatietyYield)
        {
            _m_satietyYield -= _refObjSatietyYield;
            _setDirty();
        }
        internal void _minusSatietyYieldAdjustCoefficient(long _value)
        {
            _m_satietyYieldAdjustCoefficient -= _value;
            _setDirty();
        }
    }
}