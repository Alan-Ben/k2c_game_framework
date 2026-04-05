using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励界面
    /// </summary>
    public class GGUIMonoActivityStepReward : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("活动剩余时间文本")]
        public Text txtLeftTime;
        [ALHeader("阶段目标列表")]
        public GGUIMonoActivityStepRewardGrid monoStepRewardGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6005); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6005); } }
    }
}