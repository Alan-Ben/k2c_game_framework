using ChatPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChatMsgItemGuildMarsMineOccupy : _AGUIMonoChatMsgListItem
    {
        [ALHeader("内容")]
        public TextEx txtContent;
        // [ALHeader("跳转按钮")]
        // public GameObject btnJump;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1306); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1306);} }
    }
}
