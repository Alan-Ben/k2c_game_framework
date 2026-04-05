using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 倒计时事件弹窗
    /// </summary>
    public class GGUIMonoCountdownEvent : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("事件名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("倒计时控件")]
        public NPGGUIMonoCommonCountDown monoCD;
        [ALHeader("任务列表")]
        public GGUIMonoCountdownEventTaskContainer monoTaskContainer;
        [ALHeader("可领奖时需要显示的GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("可领奖时需要隐藏的GO列表")]
        public List<GameObject> goCanGetRewardHideList;
        [ALHeader("点击领奖完成任务动画")]
        public CommonAnimationSingleInfo aniFinish;
    }
}