using ALPackage;

namespace GOE
{
    /// <summary>
    /// 奇物item
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureQualityGroupItem : _AALBasicUIWndMono
    {
        [ALHeader("品质名称")]
        public TextEx txtQualityName;

        [ALHeader("奇物列表")]
        public GGUIMonoTreasureHuntTreasureItemSizeChangeableContainer monoTreasureContainer;
    }
}