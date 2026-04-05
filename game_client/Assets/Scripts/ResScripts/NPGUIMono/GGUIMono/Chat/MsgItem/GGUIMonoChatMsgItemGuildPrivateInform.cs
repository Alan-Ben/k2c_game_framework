namespace GOE
{
    public class GGUIMonoChatMsgItemGuildPrivateInform : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("公告内容")]
        public TextEx txtContent;
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1365); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1365);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1364); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1364);} }
    }
}