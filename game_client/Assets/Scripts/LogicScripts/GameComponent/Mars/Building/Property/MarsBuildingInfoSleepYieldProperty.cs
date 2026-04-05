using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoSleepYieldProperty : _AMarsBuildingInfoProperty
    {
        private long _m_sleepYield;
        private long _m_sleepYieldAdjustCoefficient;

        public MarsBuildingInfoSleepYieldProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, false)
        {
            _m_sleepYield = 0;
            _m_sleepYieldAdjustCoefficient = 0;
        }

        private protected override long _computeValue()
        {
            return _m_sleepYield * _m_sleepYieldAdjustCoefficient;
        }

        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateSleepValue();
        }

        public void _addSleepYield(long _sleepYield)
        {
            _m_sleepYield += _sleepYield;
            _setDirty();
        }
        internal void _addSleepYieldAdjustCoefficient(long _value)
        {
            _m_sleepYieldAdjustCoefficient += _value;
            _setDirty();
        }

        public void _minusSleepYield(long _sleepYield)
        {
            _m_sleepYield -= _sleepYield;
            _setDirty();
        }
        internal void _minusSleepYieldAdjustCoefficient(long _value)
        {
            _m_sleepYieldAdjustCoefficient -= _value;
            _setDirty();
        }
    }
}
