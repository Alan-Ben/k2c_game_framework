using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 猫咪气泡动画事件控制mono
    /// </summary>
    public class CatBubbleAnimationEventMono : MonoBehaviour
    {
        public void playBubbleShowAni()
        {
            WinMsg.SendMsg(WinMsgType.TRIGGER_CAT_BUBBLE_SHOW_ANI);//触发猫咪气泡显示动画
        }

        public void playBubbleHideAni()
        {
            WinMsg.SendMsg(WinMsgType.TRIGGER_CAT_BUBBLE_HIDE_ANI);//触发猫咪气泡隐藏动画
        }
    }
}