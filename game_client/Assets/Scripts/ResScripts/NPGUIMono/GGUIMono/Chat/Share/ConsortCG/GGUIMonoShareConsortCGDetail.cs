using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 家人CG分享详情弹窗
    /// </summary>
    public class GGUIMonoShareConsortCGDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("CG ShowCase")]
        public GGUIMonoCommonShowCase monoShowCaseWnd;
        [ALHeader("CG GO在舞台的下标")]
        public int showcaseIndex = 2;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1371); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1371);} }
    }
}