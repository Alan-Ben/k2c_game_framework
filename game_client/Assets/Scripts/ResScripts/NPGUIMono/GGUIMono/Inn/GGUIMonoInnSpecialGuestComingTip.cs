using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnSpecialGuestComingTip : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClick;
        [ALHeader("特殊客人的图片")]
        public RawImage imgGuestIcon;
        [ALHeader("特殊客人的名字")]
        public Text txtGuestName;

        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6432); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6432); } }
    }
}