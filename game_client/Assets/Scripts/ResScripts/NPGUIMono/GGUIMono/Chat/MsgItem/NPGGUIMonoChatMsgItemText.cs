
using ChatPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIMonoChatMsgItemText : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("气泡框父对象")]
        public Transform bubbleParent;
        
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1302); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1302);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1303); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1303);} }
    }
}