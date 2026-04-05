using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChatMsgItemSystemLogSubWnd : _AALBasicUIWndMono
    {
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("内容")]
        public Text txtContent;

        [ALHeader("背景图")] 
        public RawImage imgBanner;
        [ALHeader("跳转按钮")]
        public GameObject btnJump;
    }
}