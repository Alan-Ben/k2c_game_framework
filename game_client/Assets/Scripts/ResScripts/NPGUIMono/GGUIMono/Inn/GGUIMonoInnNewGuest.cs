using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnNewGuest : _AALBasicUIWndMono
    {
        [ALHeader("新客人的形象")]
        public RawImage imgGuestIcon;
        [ALHeader("确认新客人按钮")]
        public GameObject btnCheckGuest;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6438); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6438); } }
    }
}