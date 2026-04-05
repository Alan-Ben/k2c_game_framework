using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 聊天信息列表页面
    /// </summary>
    public class GGUIMonoChatInfoListPage : _AALBasicUIWndMono
    {
        [ALHeader("聊天室的名字")]
        public Text txtChatName;
        [ALHeader("聊天室的图标")]
        public RawImage rawImgChatIcon;
        [ALHeader("聊天列表mono")]
        public NPGGUIMonoMsgItemList monoMsgList;
        [ALHeader("聊天输入框Mono")]
        public NPGGUIMonoChatInputter monoInputter;
        [ALHeader("附加部分的尺寸")]
        public float additionContentSize = 520;
        [ALHeader("附加部分出现的动画时间")]
        public float additionContentAnimTime = 0.25f;
        

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1311); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1311);} }
    }
}