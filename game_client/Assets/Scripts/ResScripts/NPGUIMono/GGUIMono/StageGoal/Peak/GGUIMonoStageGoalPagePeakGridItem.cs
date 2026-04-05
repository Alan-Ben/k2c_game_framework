using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标-时代之巅阶段列表item
    /// </summary>
    public class GGUIMonoStageGoalPagePeakGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("大阶段标题")]
        public Text txtBigStageName;
        [ALHeader("大阶段序号")]
        public Text txtBigStageNum;
        [ALHeader("大阶段banner图")]
        public RawImage imgBigStageBanner;
        [ALHeader("查看已到达玩家按钮")]
        public GameObject btnReachDetail;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoRewardContainer;
        [ALHeader("首个到达的玩家头像")]
        public NPGGUIMonoPlayerIcon monoFirstReachPlayerIcon;
        [ALHeader("首个到达的时间")]
        public Text txtFirstReachTime;
        [ALHeader("有到达的玩家时显示的GO列表")]
        public List<GameObject> goHaveReachPlayerShowList;
        [ALHeader("有到达的玩家时隐藏的GO列表")]
        public List<GameObject> goHaveReachPlayerHideList;
        [ALHeader("可领奖时显示的GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("可领奖时隐藏的GO列表")]
        public List<GameObject> goCanGetRewardHideList;
    }
}