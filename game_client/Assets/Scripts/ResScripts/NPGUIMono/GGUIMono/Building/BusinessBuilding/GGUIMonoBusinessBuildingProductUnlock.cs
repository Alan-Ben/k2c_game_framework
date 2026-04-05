using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingProductUnlock : _AALBasicUIWndMono
    {
        [ALHeader("业务图标")]
        public RawImage imgProductIcon;
        [ALHeader("业务名称")]
        public Text txtProductName;
        [ALHeader("业务的加成")]
        public GGUIMonoCommonBonusShower monoProductBonus;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1120); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1120); } }
    }
}