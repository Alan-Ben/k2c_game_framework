using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoCureRateProperty : _AMarsBuildingInfoProperty
    {
        private long _m_cureRate;
        

        public MarsBuildingInfoCureRateProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, true)
        {
            _m_cureRate = 0;
        }
        

        private protected override long _computeValue()
        {
            long count = buildingInfo.type == EMarsBuildingType.HOSPITAL ? peopleCount : 0;
            return _m_cureRate + count * GRefdataCoreMgr.instance.npGeneral.mars_building_people_cure_rate;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdateCureRate();
        }
        
        
        internal void _addCureRate(long _cureRate)
        {
            _m_cureRate += _cureRate;
            _setDirty();
        }
        internal void _minusCureRate(long _cureRate)
        {
            _m_cureRate -= _cureRate;
            _setDirty();
        }
    }
}
