using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜宝箱分享banner
    /// </summary>
    public class GGUIMonoChatMsgItemActivityRankBox : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("子窗口父节点")]
        public Transform transParent;

        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1383); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1383); } }

        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1383); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1383); } }
    }
}
