namespace GOE
{
    public interface _ITreasureHuntSkillInfo
    {
        /// <summary>
        /// 技能id
        /// </summary>
        public long skillId { get; }
        
        /// <summary>
        /// 技能配表数据
        /// </summary>
        public TreasureHuntSkillRefObj skillRefObj { get; }
        
        /// <summary>
        /// 技能等级
        /// </summary>
        public int level { get; }

        /// <summary>
        /// 技能等级配表数据
        /// </summary>
        public TreasureHuntSkillLevelRefObj skillLevelRefObj { get; }
        
        /// <summary>
        /// 技能状态
        /// </summary>
        public ETreasureHuntSkillState skillState { get; }
        
        /// <summary>
        /// 技能点item
        /// </summary>
        public NPCommonItem skillPointItem { get; }

        /// <summary>
        /// 技能点数
        /// </summary>
        public long skillPointNum { get; }
    }
}