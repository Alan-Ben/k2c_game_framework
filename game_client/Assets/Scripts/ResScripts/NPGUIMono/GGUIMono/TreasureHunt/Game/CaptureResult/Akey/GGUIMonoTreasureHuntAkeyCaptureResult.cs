using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键探索结果窗口
    /// </summary>
    public class GGUIMonoTreasureHuntAkeyCaptureResult : _AALBasicUIWndMono
    {
        [ALHeader("总探索次数")]
        public TextEx txtTotalExploreCount;

        [ALHeader("增加经验")]
        public TextEx txtAddExp;
        
        [ALHeader("结果展示Container")]
        public GGUIMonoTreasureHuntAkeyCaptureResultContainer monoOreShow;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6814); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6814); } }
    }
}