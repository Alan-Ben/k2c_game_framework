using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾窗口
    /// </summary>
    public class GGUIMonoDialogueHistory : _AALBasicUIWndMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("对话记录加载父节点")]
        public Transform goChatHistoryParent;
        [ALHeader("对话记录列表ScrollRect")]
        public ScrollRect chatHistoryScrollRect;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(22007); } }
        public static string objName { get { return UIResPathAssistant.getObjName(22007); } }
    }
}
