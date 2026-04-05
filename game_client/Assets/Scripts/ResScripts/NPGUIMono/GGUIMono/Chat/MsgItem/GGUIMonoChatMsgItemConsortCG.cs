using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 情人CG分享信息的msgItem
    /// </summary>
    public class GGUIMonoChatMsgItemConsortCG : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("图片")]
        public RawImage texImage;
        [ALHeader("CG名称")]
        public Text txtCGName;

        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1373); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1373);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1372); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1372);} }
    }
}