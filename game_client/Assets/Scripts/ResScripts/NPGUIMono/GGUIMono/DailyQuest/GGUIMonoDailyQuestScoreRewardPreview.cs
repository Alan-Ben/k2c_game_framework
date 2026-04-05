using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 每日任务活跃积分奖励预览界面
    /// </summary>
    public class GGUIMonoDailyQuestScoreRewardPreview : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("解锁条件")]
        public Text txtUnlockDesc;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardItemContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3301); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3301); } }
    }
}