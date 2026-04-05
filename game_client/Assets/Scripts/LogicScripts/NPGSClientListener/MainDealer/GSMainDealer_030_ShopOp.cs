using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_030_ShopOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_030_ShopOp()
        : base(30, 70)
        {
            regDealer(new GSSubDealer_030_050_OnShopRefresh());
            regDealer(new GSSubDealer_030_051_OnShopItemChg());
            regDealer(new GSSubDealer_030_060_OnGiftPackRefresh());
			regDealer(new GSSubDealer_030_061_OnGiftPackBuyRecordChg());
			regDealer(new GSSubDealer_030_062_OnGiftPackBuyRecordAdd());
			regDealer(new GSSubDealer_030_063_OnGiftPackBuyRecordRemove());
        }
    }
}