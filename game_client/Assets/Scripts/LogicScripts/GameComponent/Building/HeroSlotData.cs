using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 建筑的驻扎英雄插槽数据
    /// </summary>
    public readonly struct HeroSlotData
    {
        // 所属的建筑信息
        [NotNull] private readonly BusinessBuildingInfo _m_buildingInfo;
        // 解锁这个插槽所需的员工数量
        private readonly long _m_unlockEmployeeNum;
        // 当前驻扎的英雄信息
        private readonly HeroInfo _m_heroInfo;
        
        
        public HeroSlotData([NotNull] BusinessBuildingInfo _buildingInfo, long _unlockEmployeeNum, HeroInfo _heroInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _m_unlockEmployeeNum = _unlockEmployeeNum;
            _m_heroInfo = _heroInfo;
        }
        
        
        [NotNull] private BusinessBuildingInfo buildingInfo { get { return _m_buildingInfo; } }
        public bool isUnlock { get { return _m_buildingInfo.employeeNum >= _m_unlockEmployeeNum; } }
        public bool isPlaced { get { return _m_heroInfo != null; } }
        public long unlockEmployeeNum { get { return _m_unlockEmployeeNum; } }
        public HeroInfo heroInfo { get { return _m_heroInfo; } }
    }
}