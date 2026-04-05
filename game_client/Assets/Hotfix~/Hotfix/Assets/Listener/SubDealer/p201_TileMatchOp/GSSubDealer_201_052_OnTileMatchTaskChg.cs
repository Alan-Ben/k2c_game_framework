using ALBasicProtocolPack;
using Hotfix.GS2GC.p201_TileMatchOp;

namespace Hotfix
{
    /// <summary>
    /// 三消任务数据变更
    /// </summary>
    public class GSSubDealer_201_052_OnTileMatchTaskChg : HotfixSubDealer<GS2GC_201_052_OnTileMatchTaskChg>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_201_052_OnTileMatchTaskChg _msg)
        {
            if (_msg == null)
                return;

            HotfixNPPlayer.instance.tileMatchComponent.onTileMatchTaskChg(_msg);
        }
    }
}