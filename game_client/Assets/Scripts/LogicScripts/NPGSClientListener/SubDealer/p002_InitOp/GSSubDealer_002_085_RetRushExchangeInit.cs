using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 急速兑换初始化
    /// </summary>
    public class GSSubDealer_002_085_RetRushExchangeInit : NPSubDealer<GS2GC_002_085_RetRushExchangeInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_085_RetRushExchangeInit _createProtocolObj()
        {
            return new GS2GC_002_085_RetRushExchangeInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_085_RetRushExchangeInit _msg)
        {
            NPPlayer.instance.rushExchangeComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.rushExchangeComp.retRushExchangeInit(_msg);
            });
        }
    }
}
