
using ChatPackage;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoChatMsgItemTime : _AGUIMonoChatMsgListItem
    {
        [ALHeader("时间文字")]
        public Text txtTime;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1301); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1301);} }
    }
}