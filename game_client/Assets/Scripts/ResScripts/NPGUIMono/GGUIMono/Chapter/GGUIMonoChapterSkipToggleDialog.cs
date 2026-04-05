using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带勾选框的确认弹窗
    /// </summary>
    public class GGUIMonoChapterSkipToggleDialog : _AALBasicUIWndMono
    {
        [ALHeader("左侧按钮")]
        public GameObject btnLeft;

        [ALHeader("右侧按钮")]
        public GameObject btnRight;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("勾选框mono")]
        public NPGGUIMonoCommonToggleEx monoToggle;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2107); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2107); } }
    }
}
