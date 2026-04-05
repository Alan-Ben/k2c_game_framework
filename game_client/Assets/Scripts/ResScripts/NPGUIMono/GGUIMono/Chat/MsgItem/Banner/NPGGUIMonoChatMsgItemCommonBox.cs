using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宝箱分享banner
    /// </summary>
    public class NPGGUIMonoChatMsgItemCommonBox : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("子窗口父节点")]
        public Transform transParent;

        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1381); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1381); } }

        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1381); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1381); } }
    }
}
