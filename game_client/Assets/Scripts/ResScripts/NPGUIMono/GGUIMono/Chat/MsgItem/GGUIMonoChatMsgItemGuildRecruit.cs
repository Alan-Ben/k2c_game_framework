using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChatMsgItemGuildRecruit : _ANPGGUIMonoPlayerChatMsgItem_JumpTo
    {
        [ALHeader("文本内容")]
        public TextEx txtContent;
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1367); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1367);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1366); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1366);} }
    }
}