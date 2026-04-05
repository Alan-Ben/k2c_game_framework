using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段奖励页面
    /// </summary>
    public class GGUIMonoStepRewardPage : _AALBasicUIWndMono
    {
        [ALHeader("Tab页签容器")]
        public GGUIMonoStepRewardTabItemContainer tabItemContainer;
        [ALHeader("阶段奖励item列表")]
        public GGUIMonoStepRewardItemGrid itemGrid;
        [ALHeader("结束倒计时")]
        public Text txtCountDown;
        [ALHeader("一键领取按钮")]
        public GameObject btnOneKeyGet;
        [ALHeader("当可以一键领取的时候显示")]
        public List<GameObject> showOnCanOneKeyGet;
    }
}