namespace GOE
{
    /// <summary>
    /// 奇物数据接口
    /// </summary>
    public interface _ITreasureHuntTreasureInfo
    {
        /// <summary>
        /// 奇物id
        /// </summary>
        public long treasureId { get; }

        /// <summary>
        /// 配表数据
        /// </summary>
        public TreasureHuntTreasureRefObj treasureRefObj { get; }

        /// <summary>
        /// 产出配表数据, 若为空, 代表不是产出奇物
        /// </summary>
        public TreasureHuntTreasureOutputRefObj treasureOutputRefObj { get; }
        
        /// <summary>
        /// 奇物状态
        /// </summary>
        public ETreasureHuntTreasureState treasureState { get; }
        
        /// <summary>
        /// 获取时间
        /// </summary>
        public long gainTimeMs { get; }
        
        /// <summary>
        /// 技能信息
        /// </summary>
        public _ITreasureHuntSkillInfo skillInfo { get; }
    }
}