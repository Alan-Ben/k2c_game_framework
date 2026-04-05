using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 功能详情弹窗
    /// </summary>
    public class GGUIMonoFuncDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("上一个功能按钮")]
        public GameObject btnLast;
        [ALHeader("下一个功能按钮")]
        public GameObject btnNext;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("系统图片")]
        public RawImage imgBanner;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("解锁条件描述")]
        public Text txtUnlockCondDesc;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoItemContainer;
        [ALHeader("进度文本")]
        public Text txtProgress;
        [ALHeader("任务进度条")]
        public Slider sldProgress;
        [ALHeader("功能列表")]
        public GGUIMonoFuncDetailGrid monoFuncGrid;
        [ALHeader("任务完成时进度数值颜色")]
        public Color finishProgressTextColor = Color.green;
        [ALHeader("任务未完成时进度数值颜色")]
        public Color notFinishProgressTextColor = Color.red;
        [ALHeader("未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("已解锁时需要显示的GO列表")]
        public List<GameObject> goUnlockShowList;
        [ALHeader("已解锁时解锁条件描述文本颜色")]
        public Color condDescUnlockColor = Color.green;
        [ALHeader("未解锁时解锁条件描述文本颜色")]
        public Color condDescLockColor = Color.gray;
        [ALHeader("奖励按钮状态动画列表")]
        public CommonAnimationShowTypeInfo<ECommonRewardType> btnSstateAniList;

        [ALInfo("====奖励按钮奖励预览配置====")]
        [ALHeader("奖励按钮奖励预览pathid")]
        public long rewardPreviewResPathId;
        [ALHeader("奖励按钮奖励预览标题翻译key")]
        public string rewardPreviewTitleStrKey;
        [ALHeader("奖励按钮奖励预览弹窗位置偏移")]
        public Vector2 rewardPreviewToolTipOffset;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4101); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4101); } }
    }
}
