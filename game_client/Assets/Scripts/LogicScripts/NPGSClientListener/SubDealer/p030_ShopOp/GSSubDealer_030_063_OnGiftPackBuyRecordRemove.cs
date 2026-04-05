using ALBasicProtocolPack;
using GS2GC.p030_ShopOp;

namespace GOE
{
    /// <summary>
    /// 礼包记录移除推送
    /// </summary>
    public class GSSubDealer_030_063_OnGiftPackBuyRecordRemove : NPSubDealer<GS2GC_030_063_OnGiftPackBuyRecordRemove>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_030_063_OnGiftPackBuyRecordRemove _createProtocolObj()
        {
            return new GS2GC_030_063_OnGiftPackBuyRecordRemove();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_030_063_OnGiftPackBuyRecordRemove _msg)
        {
			NPPlayer.instance.giftPackComp.onGiftPackBuyRecordRemove(_msg);
        }
    }
}