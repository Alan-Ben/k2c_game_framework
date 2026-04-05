using ChatPackage;
using Common.ChildObj;
using Common.NpChatObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 宴会邀请的分享消息
    /// </summary>
    public class NPChatMsgDinnerInviteInfo : _ANPPlayerChatMsgDetailInfo<NPCommon_ChatContent_Dinner>, _INPChatMiniShowInfo
    {
        public NPChatMsgDinnerInviteInfo() : base((int)ENPChatMsgType.DINNER_INVITE)
        {

        }

        public string getMiniSender()
        {
            return sender.getCName();
        }

        public string getMiniContent()
        {
            GDinnerTypeRefObj dinnerTypeRefObj = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef((long) content.getDinnerId());
            string dinnerName = TextTranslate.instance.getLanguage(null != dinnerTypeRefObj ? dinnerTypeRefObj.name : null);
            return TextTranslate.instance.getLanguage(TransKeyConst.dinner_chat_invite_desc, dinnerName);
        }

        public void setInfo(GDinnerInfo _dinnerInfo)
        {
            content.setDinnerId(_dinnerInfo.dinnerId);
            content.setInstanceId(_dinnerInfo.instanceId);
            content.setJoinerCount(_dinnerInfo.joinerCount);
            content.setOwnerCid(_dinnerInfo.ownerCid);
        }
    }
}
