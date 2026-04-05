using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 系统任务详情界面
    /// </summary>
    public class GGUIMonoSystemQuestDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("窗口标题")]
        public Text txtWndTitle;
        [ALHeader("任务名称")]
        public Text txtQuestName;
        [ALHeader("任务进度slider")]
        public Slider sldProgress;
        [ALHeader("进度文本")]
        public Text txtProgress;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("可领取奖励时显示的GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("可领取奖励时隐藏的GO列表")]
        public List<GameObject> goCanGetRewardHideList;
        [ALHeader("奖励粒子开始位置")]
        public RectTransform particleStartRectTransform;
        [ALHeader("领奖特效父节点")]
        public Transform getRewardSfxParent;
        [ALHeader("领奖特效id")]
        public long getRewardSfxId;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_SYSTEM_QUEST_DETAIL); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_SYSTEM_QUEST_DETAIL); } }
    }
}
