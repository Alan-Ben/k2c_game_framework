using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamEdit : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        [ALHeader("编辑队列列表")]
        public GGUIMonoMarsExploreTeamEditItemContainer monoItemContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7310); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7310); } }
    }
}