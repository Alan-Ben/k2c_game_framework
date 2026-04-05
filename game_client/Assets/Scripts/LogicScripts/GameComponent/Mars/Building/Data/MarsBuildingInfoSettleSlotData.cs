using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingInfoSettleSlotData
    {
        [NotNull] private readonly MarsBuildingInfo _m_buildingInfo;
        
        private MarsBuildingSettleLevelRefObj _m_refObj;
        private MarsBuildingSettleLevelRefObj _m_nextRefObj;
        private int _m_peopleCount;
        

        public MarsBuildingInfoSettleSlotData([NotNull] MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _updateLevel(1);
        }
        
        
        public MarsBuildingSettleLevelRefObj refObj { get { return _m_refObj; } }
        public MarsBuildingSettleLevelRefObj nextRefObj { get { return _m_nextRefObj; } }
        /// <summary>
        /// 当前派遣的人数
        /// </summary>
        public int peopleCount { get { return _m_peopleCount; } }
        /// <summary>
        /// 可派遣人数限制
        /// </summary>
        public long peopleLimit
        {
            get
            {
                // 建筑无效, 返回0
                if(!_m_buildingInfo.isEnable || !isValid())
                    return 0;
                
                return (_m_refObj?.slot_num ?? 0) * GRefdataCoreMgr.instance.npGeneral.mars_building_slot_people_count;
            }
        }
        public bool isMax { get { return _m_peopleCount >= peopleLimit; } }
        public int canSettleCount { get { return (int)(peopleLimit - _m_peopleCount); } }


        public bool isValid()
        {
            return _m_refObj != null;
        }
        

        internal void _addProperty()
        {
            _addPeopleCountProperty();
            _addPeopleSlotLimitProperty();
        }
        internal void _removeProperty()
        {
            _removePeopleCountProperty();
            _removePeopleSlotLimitProperty();
        }
        internal void _updateLevel(int _buildingLevel)
        {
            if (_m_refObj != null && _m_refObj.level == _buildingLevel)
                return;
            
            if (_m_buildingInfo._propertyAdded)
                _removePeopleSlotLimitProperty();
            
            _m_refObj = GRefdataCoreMgr.instance.getMarsBuildingSettleLevelRef(_m_buildingInfo.refObj.settle_group_id, _buildingLevel);
            do _m_nextRefObj = GRefdataCoreMgr.instance.getMarsBuildingSettleLevelRef(_m_buildingInfo.refObj.settle_group_id, ++_buildingLevel);
            while (_m_refObj != null && _m_nextRefObj != null && _m_nextRefObj.slot_num <= _m_refObj.slot_num);
            
            if (_m_buildingInfo._propertyAdded)
                _addPeopleSlotLimitProperty();
        }
        internal void _updatePeopleCount(int _count)
        {
            if (_m_peopleCount == _count)
                return;
            
            if (_m_buildingInfo._propertyAdded)
                _removePeopleCountProperty();
            
            _m_peopleCount = _count;
            
            if (_m_buildingInfo._propertyAdded)
                _addPeopleCountProperty();
        }

        private void _addPeopleCountProperty()
        {
            _m_buildingInfo.actionWithAllProperty(_property => _property?._setPeopleCount(_m_peopleCount));
        }
        private void _removePeopleCountProperty()
        {
            _m_buildingInfo.actionWithAllProperty(_property => _property?._setPeopleCount(0));
        }
        private void _addPeopleSlotLimitProperty()
        {
            if (_m_refObj == null)
                return;
            
            _m_buildingInfo.slotNumLimitProperty._addSlotNumLimit(_m_refObj.slot_num * GRefdataCoreMgr.instance.npGeneral.mars_building_slot_people_count);
        }
        private void _removePeopleSlotLimitProperty()
        {
            if (_m_refObj == null)
                return;
            
            _m_buildingInfo.slotNumLimitProperty._minusSlotNumLimit(_m_refObj.slot_num * GRefdataCoreMgr.instance.npGeneral.mars_building_slot_people_count);
        }
    }
}