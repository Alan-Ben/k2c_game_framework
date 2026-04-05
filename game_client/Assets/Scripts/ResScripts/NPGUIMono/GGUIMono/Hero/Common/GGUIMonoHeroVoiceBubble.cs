using UnityEngine;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴配音气泡附加窗口
    /// </summary>
    public class GGUIMonoHeroVoiceBubble : _AALBasicUIWndMono
    {
        [ALHeader("顾问头像")]
        public GGUIMonoHeroIconItem monoHeroIcon;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("打字机附加窗口")]
        public GGUIMonoTextTypewriter monoTypewriter;
        [ALHeader("文本显示完气泡再隐藏的延迟时间")]
        public float playDoneDelayHideBubble = 3f;
        [ALHeader("是否播放配音和气泡，false:只播放气泡，true:播放配音和气泡")]
        public bool isPlayVoiceAndBubble = false;
    }
}