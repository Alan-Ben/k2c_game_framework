using GC2GS.p030_ShopOp;

namespace GOE
{
    //商店相关
    public static class GSWriter_030_ShopOp
    {
        /// <summary>
        /// 购买商品
        /// </summary>
        public static GC2GS_030_001_ReqBuyShopItem make_001_ReqBuyShopItem(long _shopId,long _instanceId,int _count)
        {
            GC2GS_030_001_ReqBuyShopItem protocol = new GC2GS_030_001_ReqBuyShopItem(_shopId,_instanceId,_count);
            return protocol;
        }

        /// <summary>
        /// 免费刷新商店
        /// </summary>
        public static GC2GS_030_002_ReqFreeRefreshShop make_002_ReqFreeRefreshShop(long _shopId)
        {
            GC2GS_030_002_ReqFreeRefreshShop protocol = new GC2GS_030_002_ReqFreeRefreshShop(_shopId);
            return protocol;
        }

        /// <summary>
        /// 付费刷新商店
        /// </summary>
        public static GC2GS_030_003_ReqRefreshShop make_003_ReqRefreshShop(long _shopId)
        {
            GC2GS_030_003_ReqRefreshShop protocol = new GC2GS_030_003_ReqRefreshShop(_shopId);
            return protocol;
        }

        /// <summary>
        /// 到点自动刷新商店
        /// </summary>
        public static GC2GS_030_004_ReqAutoRefreshShop make_004_ReqAutoRefreshShop(long _shopId)
        {
            GC2GS_030_004_ReqAutoRefreshShop protocol = new GC2GS_030_004_ReqAutoRefreshShop(_shopId);
            return protocol;
        }


    }
}