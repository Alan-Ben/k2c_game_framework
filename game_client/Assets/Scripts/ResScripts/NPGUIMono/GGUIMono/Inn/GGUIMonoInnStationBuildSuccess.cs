using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationBuildSuccess : _AALBasicUIWndMono
    {
        [ALHeader("设施名字")]
        public Text txtName;
        [ALHeader("设施图片")]
        public RawImage imgIcon;
        [ALHeader("设施描述")]
        public Text txtDesc;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6420); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6420); } }
    }
}