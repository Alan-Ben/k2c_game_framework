using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTreasureHuntGamePlayResumeCountDown : _AALBasicUIWndMono
    {
        [ALHeader("倒计时文本")]
        public Text txtCountdown;
        [ALHeader("倒计时时间")]
        public int countdownTime = 3;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6829); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6829); } }
    }
}