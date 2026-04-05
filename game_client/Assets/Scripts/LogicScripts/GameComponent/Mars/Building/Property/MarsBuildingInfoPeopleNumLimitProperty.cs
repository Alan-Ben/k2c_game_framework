using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 人口上限属性
    /// </summary>
    public class MarsBuildingInfoPeopleNumLimitProperty : _AMarsBuildingInfoProperty
    {
        private long _m_peopleNumLimit;
        

        public MarsBuildingInfoPeopleNumLimitProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, false)
        {
            _m_peopleNumLimit = 0;
        }
        

        private protected override long _computeValue()
        {
            return _m_peopleNumLimit;
        }
        private protected override void _onDirty()
        {
            NPPlayer.instance.marsComp.needUpdatePeopleNumLimit();
        }
        

        internal void _addPeopleNumLimit(long _peopleNumLimit)
        {
            _m_peopleNumLimit += _peopleNumLimit;
            _setDirty();
        }
        internal void _minusPeopleNumLimit(long _peopleNumLimit)
        {
            _m_peopleNumLimit -= _peopleNumLimit;
            _setDirty();
        }
    }
}
