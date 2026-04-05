using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 阶段奖励宝箱
    /// </summary>
    public class GGUISubMonoTileMatchStepReward : _AHotfixBaseMono
    {
        [HotfixMono("阶段奖励积分进度条")]
        public NPGGUIMonoProgress monoScoreSlider;
        [HotfixMono("进度条变化时间")]
        public float sldChgTime;
        
        [HotfixMono("奖励宝箱父节点")]
        public Transform rewardBoxParent;
        
        [HotfixMono("奖励预览按钮")]
        public GameObject btnRewardPreview;
        
        [HotfixMono("动画")]
        public Animation ani;
        [HotfixMono("当前阶段完成动画名称")]
        public string curStepCompleteAnimName;

        [HotfixMono("活动兑换币item")]
        public GGUIMonoCommonSimpleItem monoActivityExchangeTokensItem;
        
        [HotfixMono("活动分数")]
        public TextEx txtActivityScore;

        [HotfixMono("tip管理器父节点")]
        public Transform tipMgrParent;
        
        [HotfixMono("增加活动分数tip的id")]
        public long addActivityScoreTipId = 0;
    }
}