using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 低帧率检测提示弹窗
    /// </summary>
    public class NPGGUIMonoCheckLowFrameRate : _AALBasicUIWndMono
    {
        [ALHeader("左侧按钮")]
        public GameObject btnLeft;

        [ALHeader("右侧按钮")]
        public GameObject btnRight;

        [ALHeader("左侧按钮文本")]
        public Text txtLeftBtn;

        [ALHeader("右侧按钮文本")]
        public Text txtRightBtn;

        [ALHeader("标题文本")]
        public Text txtTitle;

        [ALHeader("内容文本")]
        public Text txtContent;

        [ALHeader("勾选框描述")]
        public Text txtToggleDesc;

        [ALHeader("勾选框mono")]
        public NPGGUIMonoCommonToggleEx monoToggle;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_LOW_FRAME_RATE); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_LOW_FRAME_RATE); } }
    }
}
