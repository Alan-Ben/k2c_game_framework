using Common.HeroObj;

namespace GOE
{
    /// <summary>
    /// 藏品技能数据对象
    /// </summary>
    public class EquipSkillInfo
    {
        //藏品数据库id
        private long _m_lEquipDbId;
        //技能索引
        private int _m_iIndex;
        //当前power加成值
        private int _m_iCurValue;
        //上次重塑power加成值
        private int _m_iPendingValue;
        //普通重塑次数
        private int _m_iNormalRebuildNum;


        /// <summary>
        /// 藏品数据库id
        /// </summary>
        public long equipDbId { get { return _m_lEquipDbId; } }
        /// <summary>
        /// 技能索引
        /// </summary>
        public int index { get { return _m_iIndex; } }
        /// <summary>
        /// 当前power加成值
        /// </summary>
        public int curValue { get { return _m_iCurValue; } }
        /// <summary>
        /// 上次重塑power加成值
        /// </summary>
        public int pendingValue { get { return _m_iPendingValue; } }
        /// <summary>
        /// 普通重塑次数
        /// </summary>
        public int normalRebuildNum { get { return _m_iNormalRebuildNum; } }


        public EquipSkillInfo(Equip_SkillInfo _info, long _dbId)
        {
            _m_lEquipDbId = _dbId;
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Equip_SkillInfo _info)
        {
            if (_info == null)
                return;

            _m_iIndex = _info.getIndex();
            _m_iCurValue = _info.getValue();
            _m_iPendingValue = _info.getPendingValue();
            _m_iNormalRebuildNum = _info.getNormalRebuildNum();
        }

        /// <summary>
        /// 获取普通重塑的消耗
        /// </summary>
        /// <returns></returns>
        public NPCommonCostItem getNormalRebuildCostItem()
        {
            OpCostGroupRefObj normalOpCostGroupRefObj = GRefdataCoreMgr.instance.opCostGroupRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.equip_normal_cost_group_id);
            OpCostRefObj costRef = normalOpCostGroupRefObj.getOpCostRefObj(_m_iNormalRebuildNum);
            return costRef?.cost_item;
        }
    }
}