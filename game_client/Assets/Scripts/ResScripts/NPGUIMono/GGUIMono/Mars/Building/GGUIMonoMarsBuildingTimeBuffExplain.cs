using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildingTimeBuffExplain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7113); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7113); } }
    }
}
