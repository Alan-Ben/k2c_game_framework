
using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUIChatMsgItemShareCommonColor
    {
        [ALHeader("分享类型")]
        public EChatShareType shareType;
        [ALHeader("颜色")]
        public Color color;
    }

    public class GGUIMonoChatMsgItemShareCommon : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("分享名字")]
        public TextEx textName;
        [ALHeader("详情按钮")]
        public GameObject btnInfo;
        [ALHeader("需要改变颜色的组件列表")]
        public List<MaskableGraphic> needChangeColorList;
        [ALHeader("类型颜色表")]
        public List<GGUIChatMsgItemShareCommonColor> monoColorTypeList;
        
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1354); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1354);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1355); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1355);} }
    }
}