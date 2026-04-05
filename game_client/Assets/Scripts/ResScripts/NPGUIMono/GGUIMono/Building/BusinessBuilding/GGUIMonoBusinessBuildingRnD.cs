using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingRnD : _AALBasicUIWndMono
    {
        [ALHeader("建筑名称")]
        public Text txtBuildingName;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public GGUIMonoBusinessBuildingRnDPageTabList monoTabList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1117); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1117); } }
    }
}