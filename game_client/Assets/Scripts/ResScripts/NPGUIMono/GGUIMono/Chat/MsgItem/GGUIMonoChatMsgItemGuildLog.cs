using ChatPackage;

namespace GOE
{
    public class GGUIMonoChatMsgItemGuildLog : _AGUIMonoChatMsgListItem
    {
        [ALHeader("内容")]
        public TextEx txtContent;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1363); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1363);} }
    }
}