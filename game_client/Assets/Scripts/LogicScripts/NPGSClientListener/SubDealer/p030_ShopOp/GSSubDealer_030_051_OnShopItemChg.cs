
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_030_051_OnShopItemChg : NPSubDealer<GS2GC.p030_ShopOp.GS2GC_030_051_OnShopItemChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p030_ShopOp.GS2GC_030_051_OnShopItemChg _createProtocolObj()
        {
            return new GS2GC.p030_ShopOp.GS2GC_030_051_OnShopItemChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p030_ShopOp.GS2GC_030_051_OnShopItemChg _msg)
        {
            if(_msg == null)
                return;

            NPPlayer.instance.shopComp.retOnShopItemChg(_msg.getShopRefId(),_msg.getShopItem());
        }
    }
}
