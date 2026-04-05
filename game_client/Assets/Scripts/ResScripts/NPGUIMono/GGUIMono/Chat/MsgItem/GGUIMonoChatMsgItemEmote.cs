
using ChatPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChatMsgItemEmote : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("气泡框父对象")]
        public Transform bubbleParent;
        [ALHeader("计算高度的差值")]
        public float offsetHeight = 100;
        
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1356); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1356);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1357); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1357);} }
    }
}