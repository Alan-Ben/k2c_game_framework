using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 聊天表情页面
    /// </summary>
    public class GGUIMonoChatEmotePage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("表情grid")]
        public GGUIMonoChatEmoteGrid emoteGrid;
        [ALHeader("表情组container")]
        public GGUIMonoChatEmoteGroupContainer emoteGroupContainer;
        
    }
}