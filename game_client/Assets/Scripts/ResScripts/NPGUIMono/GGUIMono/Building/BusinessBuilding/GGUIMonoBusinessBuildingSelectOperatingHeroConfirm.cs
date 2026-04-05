using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoBusinessBuildingSelectOperatingHeroConfirm : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("要替换建筑的伙伴列表")]
        public GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainer monoHeroContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1115); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1115); } }
    }
}