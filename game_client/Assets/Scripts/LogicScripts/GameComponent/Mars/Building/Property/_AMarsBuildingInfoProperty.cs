using JetBrains.Annotations;

namespace GOE
{
    public abstract class _AMarsBuildingInfoProperty
    {
        [NotNull] private readonly MarsBuildingInfo _m_buildingInfo;
        private readonly bool _m_isPeopleRelated;

        private int _m_peopleCount;
        private bool _m_isDirty;
        private long _m_value;
        
        
        protected _AMarsBuildingInfoProperty(MarsBuildingInfo _buildingInfo, bool _isPeopleRelated)
        {
            _m_buildingInfo = _buildingInfo;
            _m_isPeopleRelated = _isPeopleRelated;
            _m_peopleCount = 0;
            _m_isDirty = true;
            _m_value = 0;
        }
        
        
        [NotNull] public MarsBuildingInfo buildingInfo { get { return _m_buildingInfo; } }
        public int peopleCount { get { return _m_peopleCount; } }
        public long value
        {
            get
            {
                if (_m_isDirty)
                    _m_value = _computeValue();
                return _m_value;
            }
        }


        internal void _setDirty()
        {
            _m_isDirty = true;
            _onDirty();
        }
        internal void _setPeopleCount(int _value)
        {
            _m_peopleCount = _value;
            if (_m_isPeopleRelated)
                _setDirty();
        }
        
        
        private protected abstract long _computeValue();
        private protected abstract void _onDirty();
    }
}