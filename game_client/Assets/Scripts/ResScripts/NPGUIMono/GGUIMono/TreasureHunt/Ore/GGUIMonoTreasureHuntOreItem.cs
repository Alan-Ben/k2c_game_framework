using ALPackage;

namespace GOE
{
    /// <summary>
    /// 矿石Item
    /// </summary>
    public class GGUIMonoTreasureHuntOreItem : _TALUGUIMonoGridItem
    {
        [ALHeader("矿石信息子窗口")]
        public GGUIMonoTreasureHuntOreInfo oreInfoMono;
        
        [ALHeader("红点")]
        public NPGGUIMonoCommonRedTip monoRedTip;
    }
}