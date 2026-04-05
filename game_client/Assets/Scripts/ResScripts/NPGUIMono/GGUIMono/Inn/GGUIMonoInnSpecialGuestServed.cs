using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnSpecialGuestServed : _AALBasicUIWndMono
    {
        [ALHeader("特殊客人的图片")]
        public RawImage imgGuestIcon;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6433); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6433); } }
    }
}