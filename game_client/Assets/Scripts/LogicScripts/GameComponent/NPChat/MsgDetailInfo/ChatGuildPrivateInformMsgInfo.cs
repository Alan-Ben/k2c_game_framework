
using ALPackage;
using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 联盟私聊公告信息
    /// </summary>
    public class ChatGuildPrivateInformMsgInfo : _AMsgDetailInfo<NPCommon_ChatContent_Text, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        public ChatGuildPrivateInformMsgInfo() : base((int) ENPChatMsgType.GUILD_PRIVATE_INFORM)
        {
        }
        
        /// <summary>
        /// 是否是我自己的消息
        /// </summary>
        public override bool isMyMsg { get { return sender.getCid() == NPPlayer.instance.playerInfo.CID; } }
        
        /// <summary>
        /// 设置消息发送者为我自己
        /// </summary>
        public void setSenderIsMyself()
        {
            sender.setCName(NPPlayer.instance.playerInfo.PlayerName);
            sender.setCid(NPPlayer.instance.playerInfo.CID);
            sender.setIconId(NPPlayer.instance.playerInfo.curIcon.baseData.sub_id);
            sender.setIconBgkId(NPPlayer.instance.playerInfo.curIconBgk.baseData.sub_id);
            sender.setBubbleId(NPPlayer.instance.playerInfo.getCurrentBubbleId());
            sender.setVipLvl((int)NPPlayer.instance.playerInfo.getCurrentVIPLvl());
            sender.setPlayerSkinId(NPPlayer.instance.playerInfo.getCurrentSkinId());
            sender.setCurTitle(NPPlayer.instance.titleComp.curTitle);
            sender.setIsShow(NPPlayer.instance.titleComp.isShowOthers);
        }

        /// <summary>
        /// 设置消息内容
        /// </summary>
        public void setContent(string _text)
        {
            content.setContent(_text);
        }
        /// <inheritdoc/>
        public string getMiniSender()
        {
            return sender.getCName();
        }
        /// <inheritdoc/>
        public string getMiniContent()
        {
            return content.getContent();
        }
    }
}