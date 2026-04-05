using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoChatMsgItemTextBubble : _AALBasicUIWndMono
    {
        [ALHeader("文字内容")]
        public Text txtText;
        [ALHeader("修正文字背景的mono")]
        public TextSizeFixMonoV2 monoSizeFix;
        
        [ALHeader("加载的GO的父节点，加载go和文字消息只会有一个")]
        public Transform loadGoParent;

        [ALHeader("文字消息的时候显示")]
        public List<GameObject> textMsgShow;
        [ALHeader("加载的预制体的消息时候显示")]
        public List<GameObject> prefabMsgShow;
    }
}