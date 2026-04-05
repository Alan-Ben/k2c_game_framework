using ALPackage;

namespace GOE
{
    /// <summary>
    /// 奇物item
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureItem : _TALUGUIMonoGridItem
    {
        [ALHeader("奇物信息子窗口")]
        public GGUIMonoTreasureHuntTreasureInfo treasureInfoMono;
        
        [ALHeader("红点")]
        public NPGGUIMonoCommonRedTip monoRedTip;
    }
}