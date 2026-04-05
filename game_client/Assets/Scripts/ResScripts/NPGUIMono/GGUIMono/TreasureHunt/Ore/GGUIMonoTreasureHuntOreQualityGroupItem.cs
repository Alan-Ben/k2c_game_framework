using ALPackage;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 矿石品质组item
    /// </summary>
    public class GGUIMonoTreasureHuntOreQualityGroupItem : _AALBasicUIWndMono
    {
        [ALHeader("品质名称")]
        public TextEx txtQualityName;

        [ALHeader("矿石列表")]
        public GGUIMonoTreasureHuntOreItemSizeChangeableContainer monoOreContainer;
    }
}