using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoPlayerDailyRewardPreview : _AALBasicUIWndMono
    {
        public GameObject btnClose;
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1716); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1716); } }
    }
}