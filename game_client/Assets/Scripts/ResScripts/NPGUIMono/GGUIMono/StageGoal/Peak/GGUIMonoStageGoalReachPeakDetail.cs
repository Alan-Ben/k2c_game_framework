using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标到达时代之巅详情弹窗
    /// </summary>
    public class GGUIMonoStageGoalReachPeakDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("大阶段标题")]
        public Text txtTitle;
        [ALHeader("排名列表")]
        public GGUIMonoStageGoalReachPeakDetailContainer monoDetailContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5017); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5017); } }
    }
}