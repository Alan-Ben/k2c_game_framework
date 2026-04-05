using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public class GGUIMonoTileMatchStepRewardBox : _AHotfixBaseMono
    {
        [HotfixMono("可领取奖励时显示")]
        public List<GameObject> hasRewardCanDrawShow;

        [HotfixMono("没有阶段奖励时显示")]
        public List<GameObject> noRewardCanDrawShow;

        [HotfixMono("可领取奖励数量")]
        public TextEx txtCanDrawReward;
        
        [HotfixMono("领取奖励按钮")]
        public GameObject btnDraw;
    }
}