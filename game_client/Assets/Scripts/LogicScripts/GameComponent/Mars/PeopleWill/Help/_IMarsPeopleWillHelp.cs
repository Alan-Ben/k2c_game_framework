using Common.MarsObj;

namespace GOE
{
    /// <summary>
    /// 民意求助
    /// </summary>
    public interface _IMarsPeopleWillHelp
    {
        /// <summary>
        /// 求助实例id
        /// </summary>
        long instanceId { get; }

        /// <summary>
        /// 求助配表id
        /// </summary>
        long refId { get; }

        /// <summary>
        /// 求助配表数据
        /// </summary>
        MarsPeopleHelpRefObj refObj { get; }
        
        /// <summary>
        /// npc配表数据
        /// </summary>
        NPNPCRefObj npcRefObj { get; }

        /// <summary>
        /// 求助状态
        /// </summary>
        EMarsPopularWillHelpState state { get; }

        void update(Mars_Help _serverHelpInfo);
        
        /// <summary>
        /// 求助处理方法
        /// </summary>
        void deal();
    }
}