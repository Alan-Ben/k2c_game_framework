using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带勾选框的消耗确认弹窗
    /// </summary>
    public class NPGGUIMonoCommonCostToggleDialog : _AALBasicUIWndMono
    {
        [ALHeader("消耗道具mono")]
        public NPGGUIMonoCommonItem monoCostItem;

        [ALHeader("确认按钮")]
        public GameObject btnConfirm;

        [ALHeader("取消按钮")]
        public GameObject btnCancel;

        [ALHeader("标题文本")]
        public Text txtTitle;

        [ALHeader("内容文本")]
        public Text txtContent;

        [ALHeader("勾选框描述")]
        public Text txtToggleDesc;

        [ALHeader("勾选框mono")]
        public NPGGUIMonoCommonToggleEx monoToggle;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_COST_TOGGLE_DIALOG); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_COST_TOGGLE_DIALOG); } }
    }
}
