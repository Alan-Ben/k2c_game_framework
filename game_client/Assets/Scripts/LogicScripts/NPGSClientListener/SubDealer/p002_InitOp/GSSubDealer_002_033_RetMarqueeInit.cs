using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 请求跑马灯初始化
    /// </summary>
    public class GSSubDealer_002_033_RetMarqueeInit : NPSubDealer<GS2GC_002_033_RetMarqueeInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_033_RetMarqueeInit _createProtocolObj()
        {
            return new GS2GC_002_033_RetMarqueeInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_033_RetMarqueeInit _msg)
        {
            NPPlayer.instance.marqueeComp.retMarqueeInit(_msg);
        }
    }
}