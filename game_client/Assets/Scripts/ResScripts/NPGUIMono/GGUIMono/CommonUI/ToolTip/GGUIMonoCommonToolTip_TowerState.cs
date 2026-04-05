using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 爬塔当前状态Tip
    /// </summary>
    public class GGUIMonoCommonToolTip_TowerState : NPGGUIMonoCommonToolTip
    {
        [ALHeader("当前挑战进度")]
        public TextEx txtLevelName;
        [ALHeader("总收益加成")]
        public TextEx txtEarnBonus;
        [ALHeader("单日收益爬塔币")]
        public TextEx txtDailyCoins;
    }
}