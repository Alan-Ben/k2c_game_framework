using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnCashRegisterUpgradeTip : _AALBasicUIWndMono
    {
        [ALHeader("收银台图标")]
        public RawImage imgIcon;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6444); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6444); } }
    }
}
