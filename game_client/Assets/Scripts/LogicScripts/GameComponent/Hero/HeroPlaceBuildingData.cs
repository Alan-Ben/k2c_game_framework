using System;
using Common.HeroObj;

namespace GOE
{
    /// <summary>
    /// 伙伴驻扎在建筑上的数据
    /// </summary>
    public struct HeroPlaceBuildingData : IComparable<HeroPlaceBuildingData>
    {
        // 驻扎的目标建筑 id
        private readonly long _m_buildingId;
        // 驻扎的序列号（越晚驻扎的数字越大，主要用于客户端排序）
        private readonly long _m_placeSerialize;
        
        public HeroPlaceBuildingData(long _buildingId, int _placeSerialize)
        {
            _m_buildingId = _buildingId;
            _m_placeSerialize = _placeSerialize;
        }
        public HeroPlaceBuildingData(Hero_PlaceInfo _placeInfo)
        {
            if (_placeInfo == null)
            {
                _m_buildingId = 0;
                _m_placeSerialize = 0;
                return;
            }
            _m_buildingId = _placeInfo.getBuildingId();
            _m_placeSerialize = _placeInfo.getSerial();
        }


        /// <summary>
        /// 是否有驻扎建筑
        /// </summary>
        public bool isPlaced { get { return _m_buildingId != 0; } }
        /// <summary>
        /// 驻扎的建筑 id
        /// </summary>
        public long buildingId { get { return _m_buildingId; } }
        /// <summary>
        /// 驻扎的序列号（越晚驻扎的数字越大，主要用于客户端排序）
        /// </summary>
        public long placeSerialize { get { return _m_placeSerialize; } }


        public int CompareTo(HeroPlaceBuildingData _other)
        {
            return _m_placeSerialize.CompareTo(_other._m_placeSerialize);
        }
    }
}