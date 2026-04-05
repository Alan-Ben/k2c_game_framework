using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 活动商店商品购买变更
    /// </summary>
    public class GSSubDealer_017_060_OnActivityShopItemBuy : NPSubDealer<GS2GC_017_060_OnActivityShopItemBuy>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_017_060_OnActivityShopItemBuy _createProtocolObj()
        {
            return new GS2GC_017_060_OnActivityShopItemBuy();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_060_OnActivityShopItemBuy _msg)
        {
			NPPlayer.instance.commonActivityComp.onActivityShopItemBuy(_msg);
        }
    }
}