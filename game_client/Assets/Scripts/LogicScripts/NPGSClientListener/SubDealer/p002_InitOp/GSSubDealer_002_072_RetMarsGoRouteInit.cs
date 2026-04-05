using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 火星-前往火星数据初始化
    /// </summary>
    public class GSSubDealer_002_072_RetMarsGoRouteInit : NPSubDealer<GS2GC_002_072_RetMarsGoRouteInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_072_RetMarsGoRouteInit _createProtocolObj()
        {
            return new GS2GC_002_072_RetMarsGoRouteInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_072_RetMarsGoRouteInit _msg)
        {
			NPPlayer.instance.marsComp.dealPreInitFunc(() =>
            { 
                NPPlayer.instance.marsComp.goToSubComponent.retMarsGoRouteInit(_msg);
            });
        }
    }
}