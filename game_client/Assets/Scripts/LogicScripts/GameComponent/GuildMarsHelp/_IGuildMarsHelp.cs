using Common.GuildEnum;

namespace GOE
{
    public interface _IGuildMarsHelp
    {
        // 联盟帮助类型
        public EGuildMarsHelpObjType helpObjType { get; }
        
        // 联盟帮助对象id
        public long guildHelpObjId { get; }
        
        // 联盟帮助实例id
        public long guildHelpId { get; } 

        /// <summary>
        /// 联盟帮助状态变化
        /// </summary>
        /// <param name="_guildHelpSecs"></param>
        public void onGuildHelpChg(long _guildHelpId, long _guildHelpSecs);

        /// <summary>
        /// 联盟帮助结束
        /// </summary>
        public void onGuildHelpDel();
    }
}