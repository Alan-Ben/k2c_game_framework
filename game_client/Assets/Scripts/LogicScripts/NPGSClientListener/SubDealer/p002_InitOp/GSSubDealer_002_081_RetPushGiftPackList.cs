using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 初始化推送礼包信息
    /// </summary>
    public class GSSubDealer_002_081_RetPushGiftPackList : NPSubDealer<GS2GC_002_081_RetPushGiftPackList>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_081_RetPushGiftPackList _createProtocolObj()
        {
            return new GS2GC_002_081_RetPushGiftPackList();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_081_RetPushGiftPackList _msg)
        {
            NPPlayer.instance.pushGiftComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.pushGiftComp.retPushGiftPackList(_msg);
            });
        }
    }
}
