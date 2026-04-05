using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnNewGuestUnlockSuccess : _AALBasicUIWndMono
    {
        [ALHeader("新客人名字")]
        public Text txtGuestName;
        [ALHeader("新客人 icon ")]
        public RawImage imgGuestIcon;
        [ALHeader("客人描述")]
        public Text txtGuestDesc;
        [ALHeader("客人效果")]
        public Text txtGuestEffect;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6436); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6436); } }
    }
}