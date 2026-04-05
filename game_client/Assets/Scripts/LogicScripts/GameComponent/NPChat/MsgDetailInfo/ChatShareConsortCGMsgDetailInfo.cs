using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 聊天妃子CG分享信息
    /// </summary>
    public class ChatShareConsortCGMsgDetailInfo : _AMsgDetailInfo<NPCommon_ChatContent_ConsortCGShare, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        public ChatShareConsortCGMsgDetailInfo() : base((int) ENPChatMsgType.SHARE_CONSORT_CG)
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
        /// 设置消息内容...
        /// </summary>
        public void setContent(ConsortCgInfo gottenConsortInfo)
        {
            if(gottenConsortInfo == null)
                return;
            
            content.setCgId(gottenConsortInfo.cgId);
        }

        public string getMiniSender()
        {
            return sender.getCName();
        }

        public string getMiniContent()
        {
            ConsortCGRefObj consortCGRefObj = GRefdataCoreMgr.instance.consortCGRefCore.getRef(content.getCgId());
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_consortCG_str, TextTranslate.instance.getLanguage(consortCGRefObj?.name));
        }
    }
}