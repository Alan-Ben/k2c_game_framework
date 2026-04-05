using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 初始化活动货币列表
    /// </summary>
    public class GSSubDealer_002_064_RetActivityCurrencyList : NPSubDealer<GS2GC_002_064_RetActivityCurrencyList>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_064_RetActivityCurrencyList _createProtocolObj()
        {
            return new GS2GC_002_064_RetActivityCurrencyList();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_064_RetActivityCurrencyList _msg)
        {
            NPPlayer.instance.commonActivityComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.commonActivityComp.retActivityCurrencyList(_msg);
            });
        }
    }
}