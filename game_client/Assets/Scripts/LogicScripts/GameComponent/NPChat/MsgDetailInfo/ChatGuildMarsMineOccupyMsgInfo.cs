using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 联盟成员火星矿被攻击信息
    /// </summary>
    public class ChatGuildMarsMineOccupyMsgInfo : _AMsgDetailInfo<ChatContent_GuildMarsMineAttackShare, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        public ChatGuildMarsMineOccupyMsgInfo() : base((int)ENPChatMsgType.GUILD_MARS_MINE_OCCUPY)
        {
        }

        /// <summary>
        /// 是否是我自己的消息
        /// </summary>
        public override bool isMyMsg { get { return sender.getCid() == NPPlayer.instance.playerInfo.CID; } }

        /// <inheritdoc/>
        public string getMiniSender()
        {
            return sender.getCName();
        }

        /// <inheritdoc/>
        public string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(
                TransKeyConst.mars_explore_guildMineUnderAttack_guildName_attackerName_name,
                content.getAttackGuildSimpleName(),
                content.getAttackCname(),
                content.getCname()
            );
        }
    }
}
