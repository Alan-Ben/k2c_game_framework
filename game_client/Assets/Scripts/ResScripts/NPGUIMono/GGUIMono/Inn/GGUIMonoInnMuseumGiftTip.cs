using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnMuseumGiftTip : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("自动关闭的时间")]
        public float autoCloseDelay = 3.0f;

        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6434); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6434); } }
    }
}