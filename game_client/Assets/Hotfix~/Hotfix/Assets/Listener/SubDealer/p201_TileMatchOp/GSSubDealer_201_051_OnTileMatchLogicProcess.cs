using ALBasicProtocolPack;
using Hotfix.GS2GC.p201_TileMatchOp;

namespace Hotfix
{
    /// <summary>
    /// 三消逻辑处理
    /// </summary>
    public class GSSubDealer_201_051_OnTileMatchLogicProcess : HotfixSubDealer<GS2GC_201_051_OnTileMatchLogicProcess>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_201_051_OnTileMatchLogicProcess _msg)
        {
            if (_msg == null)
                return;

            HotfixNPPlayer.instance.tileMatchComponent.onTileMatchLogicProcess(_msg);
        }
    }
}