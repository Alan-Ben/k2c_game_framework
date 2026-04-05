using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamUnlockDesc : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(0); } }
        public static string objName { get { return UIResPathAssistant.getObjName(0); } }
    }
}