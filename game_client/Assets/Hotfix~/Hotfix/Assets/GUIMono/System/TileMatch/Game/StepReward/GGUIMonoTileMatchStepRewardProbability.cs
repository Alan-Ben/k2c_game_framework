using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励概率窗口
    /// </summary>
    public class GGUIMonoTileMatchStepRewardProbability : _AHotfixBaseMono
    {
        [HotfixMono("奖池组Container")]
        public GGUIHotfixCommonMono monoJackpotGroupContainer;

        [HotfixMono("关闭按钮")]
        public GameObject btnClose;
    }
}