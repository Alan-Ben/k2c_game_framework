using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 功能预告页面
    /// </summary>
    public class GGUIMonoFuncPreviewPage : _AALBasicUIWndMono
    {
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("解锁条件描述")]
        public Text txtUnlockCondDesc;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("任务进度文本")]
        public Text txtProgress;
        [ALHeader("任务进度条")]
        public Slider sldProgress;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("展示列表按钮")]
        public GameObject btnShowList;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("领取按钮")]
        public GameObject btnGetReward;
        [ALHeader("可领取时需要展示的GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("可领取时需要隐藏的GO列表")]
        public List<GameObject> goCanGetRewardHideList;

        [ALHeader("任务全部完成时需要显示的GO列表")]
        public List<GameObject> goFinishAllQuestShowList;
        [ALHeader("任务全部完成时需要隐藏的GO列表")]
        public List<GameObject> goFinishAllQuestHideList;

        [ALHeader("粒子开始位置")]
        public RectTransform particleStartRectTransform;

        [ALHeader("任务完成时进度数值颜色")]
        public Color finishProgressTextColor = Color.green;
        [ALHeader("任务未完成时进度数值颜色")]
        public Color notFinishProgressTextColor = Color.red;
    }
}

