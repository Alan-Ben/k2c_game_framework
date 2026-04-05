using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡前进消耗详情提示窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_ChapterAdvanceLossDegressDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("大臣总战力")]
        public TextEx heroTotalPower;

        [ALHeader("消耗比例")]
        public TextEx txtCostRatio;
        
        [ALHeader("消耗比例icon")]
        public RawImage costRatioIcon;
    }
}