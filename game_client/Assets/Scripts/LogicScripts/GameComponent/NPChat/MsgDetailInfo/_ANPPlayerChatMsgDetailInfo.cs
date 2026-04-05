using ALBasicProtocolPack;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    public abstract class _ANPPlayerChatMsgDetailInfo<T_CONTENT> : _AMsgDetailInfo<T_CONTENT, NPCommon_ChatPlayerContent>
        where T_CONTENT : _IALProtocolStructure, new()
    {
        protected _ANPPlayerChatMsgDetailInfo(int _msgType) : base(_msgType)
        {

        }

        public override bool isMyMsg { get { return sender.getCid() == NPPlayer.instance.playerInfo.CID; } }

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
    }
}
