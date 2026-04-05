using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildingEnergyYieldDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        [ALHeader("列表对象")]
        public GGUIMonoMarsEnergyYieldDetailGrid monoGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7117); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7117); } }
    }
}
