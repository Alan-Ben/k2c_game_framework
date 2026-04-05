using ALBasicProtocolPack;
using Hotfix.GS2GC.p201_TileMatchOp;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励数据变更
    /// </summary>
    public class GSSubDealer_201_053_OnTileMatchStepRewardChg : HotfixSubDealer<GS2GC_201_053_OnTileMatchStepRewardChg>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_201_053_OnTileMatchStepRewardChg _msg)
        {
            if (_msg == null)
                return;

            HotfixNPPlayer.instance.tileMatchComponent.onTileMatchStepRewardChg(_msg);
        }
    }
}