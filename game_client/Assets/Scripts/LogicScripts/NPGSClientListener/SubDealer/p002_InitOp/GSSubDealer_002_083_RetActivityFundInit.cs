using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_002_083_RetActivityFundInit : NPSubDealer<GS2GC_002_083_RetActivityFundInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_083_RetActivityFundInit _createProtocolObj()
        {
            return new GS2GC_002_083_RetActivityFundInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_083_RetActivityFundInit _msg)
        {
			
        }
    }
}