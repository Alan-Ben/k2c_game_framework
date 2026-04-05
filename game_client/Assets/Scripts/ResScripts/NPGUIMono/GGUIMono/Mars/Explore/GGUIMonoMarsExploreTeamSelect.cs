using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamSelect : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("队伍列表")]
        public GGUIMonoMarsExploreTeamSelectContainer monoSelectContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7403); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7403); } }
    }
}