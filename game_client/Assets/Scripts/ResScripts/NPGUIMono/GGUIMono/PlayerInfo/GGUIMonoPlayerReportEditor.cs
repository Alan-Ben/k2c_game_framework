using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 举报弹窗
    /// </summary>
    public class GGUIMonoPlayerReportEditor : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("举报内容输入框")]
        public InputField reportInputField;
        [ALHeader("发送举报按钮")]
        public GameObject btnSend;
        [ALHeader("输入字数显示")]
        public Text txtInputCount;
        [ALHeader("最大输入字数")]
        public int maxInputNum = 300;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1388); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1388); } }
    }
}
