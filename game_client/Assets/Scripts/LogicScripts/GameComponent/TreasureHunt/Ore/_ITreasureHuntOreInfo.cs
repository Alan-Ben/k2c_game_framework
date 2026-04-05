namespace GOE
{
    /// <summary>
    /// 矿石数据接口
    /// </summary>
    public interface _ITreasureHuntOreInfo
    {
        /// <summary>
        /// 矿石id
        /// </summary>
        public long oreId { get; }
        
        /// <summary>
        /// 配表数据
        /// </summary>
        public TreasureHuntOreRefObj oreRefObj { get; }

        /// <summary>
        /// 矿石状态
        /// </summary>
        public ETreasureHuntOreState oreState { get; }

        /// <summary>
        /// 矿石数量
        /// </summary>
        public int num { get; }
        
        /// <summary>
        /// 矿石质量
        /// </summary>
        public int mass { get; }

        /// <summary>
        /// 获取时间
        /// </summary>
        public long getTimeMs { get; }
        
        /// <summary>
        /// 普通技能信息
        /// </summary>
        public _ITreasureHuntSkillInfo normalSkillInfo { get; }
        
        /// <summary>
        /// 高级技能信息
        /// </summary>
        public _ITreasureHuntSkillInfo advanceSkillInfo { get; }
    }
}