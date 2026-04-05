using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoTowerBattleHPTip : _AALBasicUIWndMono
    {
        [ALHeader("正常显示动画名字")]
        public string oneTimesAniName;
        [ALHeader("10倍速动画名字")]
        public string tenTimesAniName;
        [ALHeader("血量文本")]
        public Text txtHp;
    }
}
