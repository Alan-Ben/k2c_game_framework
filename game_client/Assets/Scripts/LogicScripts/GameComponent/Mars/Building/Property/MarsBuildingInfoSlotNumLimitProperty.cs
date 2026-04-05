using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoSlotNumLimitProperty : _AMarsBuildingInfoProperty
    {
        private long _m_slotNumLimit;


        public MarsBuildingInfoSlotNumLimitProperty([NotNull] MarsBuildingInfo _buildingInfo)
            : base(_buildingInfo, false)
        {
            _m_slotNumLimit = 0;
        }


        private protected override long _computeValue()
        {
            return _m_slotNumLimit;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateSettleSlotPeopleLimit();
        }


        internal void _addSlotNumLimit(long _slotNumLimit)
        {
            _m_slotNumLimit += _slotNumLimit;
            _setDirty();
        }
        internal void _minusSlotNumLimit(long _slotNumLimit)
        {
            _m_slotNumLimit -= _slotNumLimit;
            _setDirty();
        }
    }
}
