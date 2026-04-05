namespace GOE
{
    /// <summary>
    /// 民意信件
    /// </summary>
    public interface _IMarsPeopleWillLetter
    {
        /// <summary>
        /// 信件实例id
        /// </summary>
        long instanceId { get; }

        /// <summary>
        /// 信件配表id
        /// </summary>
        long refId { get; }

        /// <summary>
        /// 信件配表数据
        /// </summary>
        MarsPeopleLetterRefObj refObj { get; }
        
        /// <summary>
        /// npc配表数据
        /// </summary>
        NPNPCRefObj npcRefObj { get; }
        
        /// <summary>
        /// 是否已处理
        /// </summary>
        bool isDealed { get; }
    }
}