using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星确认开始前往界面
    /// </summary>
    public class GGUIMonoMarsGoToStartConfirm : _ANPBasicUIWndResBarMono
    {
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7001); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7001); } }
    }
}
