using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntCaptureResult : _AALBasicUIWndMono
    {
        [ALHeader("收获物列表")]
        public GGUIMonoTreasureHuntCaptureHarvestItemContainer monoHarvestContainer;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6831); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6831); } }
    }
}