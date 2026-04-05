namespace GOE
{
    public interface _IEveningDungeonHeroFightInfo
    {
        public long heroId { get; }
        
        /// <summary>
        /// 大臣卡牌显示信息
        /// </summary>
        public _IHeroCardShow heroCardShow { get; }
        
        /// <summary>
        /// 大臣攻击力
        /// </summary>
        public long fightATK { get; }

        /// <summary>
        /// 大臣出战上限次数
        /// </summary>
        public int fightMaxCount { get; }
        
        /// <summary>
        /// 大臣当前出战次数
        /// </summary>
        public int nowFightCount { get; }
    }
}