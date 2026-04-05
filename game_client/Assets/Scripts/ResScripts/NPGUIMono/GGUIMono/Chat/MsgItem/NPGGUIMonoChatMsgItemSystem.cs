
using ChatPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoChatMsgItemSystem : _AGUIMonoChatMsgListItem
    {
        [ALHeader("气泡框父对象")]
        public Transform bubbleParent;
        
        [ALHeader("头像")]
        public RawImage imgIcon;//头像
        [ALHeader("头像框")]
        public RawImage imgIconBgk;//头像框
        [ALHeader("名字")]
        public TextEx txtName;//名字
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1304); } }
        public static string systemObjName { get { return UIResPathAssistant.getObjName(1304); } }
    }
}