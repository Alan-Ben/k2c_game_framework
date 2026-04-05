using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoPowerProperty : _AMarsBuildingInfoProperty
    {
        private long _m_powerValue;
        
        
        public MarsBuildingInfoPowerProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, false)
        {
        }
        
        
        private protected override long _computeValue()
        {
            return _m_powerValue;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdatePower();
        }
        internal void addValue(long _marsPowerValue)
        {
            _m_powerValue += _marsPowerValue;
            _setDirty();
        }
        internal void removeValue(long _marsPowerValue)
        {
            _m_powerValue -= _marsPowerValue;
            _setDirty();
        }
    }
}