using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoComfortYieldProperty : _AMarsBuildingInfoProperty
    {
        private long _m_comfortYield;
        private long _m_comfortYieldAdjustCoefficient;


        public MarsBuildingInfoComfortYieldProperty([NotNull] MarsBuildingInfo _buildingInfo)
            : base(_buildingInfo, false)
        {
            _m_comfortYield = 0;
            _m_comfortYieldAdjustCoefficient = 0;
        }


        private protected override long _computeValue()
        {
            return _m_comfortYield * _m_comfortYieldAdjustCoefficient;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateComfortValue();
        }
        
        
        internal void _addComfortYield(long _comfortYield)
        {
            _m_comfortYield += _comfortYield;
            _setDirty();
        }
        internal void _addComfortYieldAdjustCoefficient(long _value)
        {
            _m_comfortYieldAdjustCoefficient += _value;
            _setDirty();
        }
        internal void _minusComfortYield(long _comfortYield)
        {
            _m_comfortYield -= _comfortYield;
            _setDirty();
        }
        internal void _minusComfortYieldAdjustCoefficient(long _value)
        {
            _m_comfortYieldAdjustCoefficient -= _value;
            _setDirty();
        }
    }
}