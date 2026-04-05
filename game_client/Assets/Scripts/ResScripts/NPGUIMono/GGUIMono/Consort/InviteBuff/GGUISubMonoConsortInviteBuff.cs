using System.Drawing;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子邀约buff子窗口
    /// </summary>
    public class GGUISubMonoConsortInviteBuff : _AALBasicUIWndMono
    {
        [ALHeader("指定邀约buff")]
        public GGUIMonoConsortInviteBuffItem_AssignInviteBuff assignInviteBuff;
        
        [ALHeader("卷王子嗣buff")]
        public GGUIMonoConsortInviteBuffItem_GiftedChildBuff giftedChildBuff;
    }
}