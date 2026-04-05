using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoMiniChat : _AALBasicUIWndMono
    {
        [ALHeader("频道的icon")]
        public RawImage rawImgChannelIcon;
        [ALHeader("频道的text")]
        public TextEx txtChannel;
        [ALHeader("内容")]
        public Text txtContent;
        [ALHeader("打开聊天界面的按钮")]
        public GameObject btnOpen;
        [ALHeader("默认点击打开频道")]
        public ENPChatRoomType defaultOpenChannel = ENPChatRoomType.US_SERVER;
        [ALHeader("需要监听哪几个聊天频道的声音")]
        public List<ENPChatRoomType> showRoomTypes;
    }
}