using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 宝箱分享消息的设置类
    /// </summary>
    public class NPChatCommonBoxDataSetting : _AChatDataSetting
    {
        public NPChatCommonBoxDataSetting() : base((int)ENPChatMsgType.COMM_BOX)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new NPChatMsgCommonBoxInfo();
        }
    }
}
