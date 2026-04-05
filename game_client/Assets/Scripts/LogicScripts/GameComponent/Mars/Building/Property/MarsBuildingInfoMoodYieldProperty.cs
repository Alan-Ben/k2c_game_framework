using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoMoodYieldProperty : _AMarsBuildingInfoProperty
    {
        private long _m_moodYield;
        private long _m_moodYieldAdjustCoefficient;


        public MarsBuildingInfoMoodYieldProperty([NotNull] MarsBuildingInfo _buildingInfo)
            : base(_buildingInfo, false)
        {
            _m_moodYield = 0;
            _m_moodYieldAdjustCoefficient = 0;
        }
        

        private protected override long _computeValue()
        {
            return _m_moodYield * _m_moodYieldAdjustCoefficient;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateMoodValue();
        }
        
        
        internal void _addMoodYield(long _moodYield)
        {
            _m_moodYield += _moodYield;
            _setDirty();
        }
        internal void _addMoodYieldAdjustCoefficient(long _value)
        {
            _m_moodYieldAdjustCoefficient += _value;
            _setDirty();
        }
        internal void _minusMoodYield(long _moodYield)
        {
            _m_moodYield -= _moodYield;
            _setDirty();
        }
        internal void _minusMoodYieldAdjustCoefficient(long _value)
        {
            _m_moodYieldAdjustCoefficient -= _value;
            _setDirty();
        }
    }
}
