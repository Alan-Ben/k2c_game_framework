using ALBasicProtocolPack;
using Hotfix.GS2GC.p200_HotSimpleActivityOp;

namespace Hotfix
{
    /// <summary>
    /// 万能活动商店购买物品推送
    /// </summary>
    public class GSSubDealer_200_101_OnRegularActivityShopItemBuy : HotfixSubDealer<GS2GC_200_101_OnRegularActivityShopItemBuy>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_200_101_OnRegularActivityShopItemBuy _msg)
        {
            HotfixNPPlayer.instance.regularEventComponent.onRegularActivityShopItemBuy(_msg);
        }
    }
}