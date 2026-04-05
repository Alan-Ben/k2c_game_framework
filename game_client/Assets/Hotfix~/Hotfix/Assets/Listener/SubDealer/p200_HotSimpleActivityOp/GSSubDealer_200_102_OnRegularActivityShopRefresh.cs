using ALBasicProtocolPack;
using Hotfix.GS2GC.p200_HotSimpleActivityOp;

namespace Hotfix
{
    /// <summary>
    /// 万能活动商店刷新推送
    /// </summary>
    public class GSSubDealer_200_102_OnRegularActivityShopRefresh : HotfixSubDealer<GS2GC_200_102_OnRegularActivityShopRefresh>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_200_102_OnRegularActivityShopRefresh _msg)
        {
            HotfixNPPlayer.instance.regularEventComponent.onRegularActivityShopRefresh(_msg);
        }
    }
}