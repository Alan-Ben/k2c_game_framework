using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoCommonToggleDialog_OneBtn : _AALBasicUIWndMono
    {
        [ALHeader("按钮")]
        public GameObject btn;

        [ALHeader("按钮文本")]
        public Text txtBtn;

        [ALHeader("标题文本")]
        public Text txtTitle;

        [ALHeader("内容文本")]
        public Text txtContent;

        [ALHeader("勾选框描述")]
        public Text txtToggleDesc;

        [ALHeader("勾选框mono")]
        public NPGGUIMonoCommonToggleEx monoToggle;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_TOGGLE_FORCE_DIALOG); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_TOGGLE_FORCE_DIALOG); } }
    }
}
