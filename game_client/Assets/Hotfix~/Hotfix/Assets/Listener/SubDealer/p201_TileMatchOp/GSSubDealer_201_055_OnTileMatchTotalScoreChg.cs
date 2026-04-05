using ALBasicProtocolPack;
using Hotfix.GS2GC.p201_TileMatchOp;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段可领取奖励数据变更
    /// </summary>
    public class GSSubDealer_201_055_OnTileMatchTotalScoreChg : HotfixSubDealer<GS2GC_201_055_OnTileMatchTotalScoreChg>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_201_055_OnTileMatchTotalScoreChg _msg)
        {
            if (_msg == null)
                return;

            HotfixNPPlayer.instance.tileMatchComponent.onTileMatchTotalScoreChg(_msg);
        }
    }
}