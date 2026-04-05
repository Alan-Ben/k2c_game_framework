using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 七日登录
    /// </summary>
    public class GSSubDealer_002_053_RetLoginCountInit : NPSubDealer<GS2GC_002_053_RetLoginCountInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_053_RetLoginCountInit _createProtocolObj()
        {
            return new GS2GC_002_053_RetLoginCountInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_053_RetLoginCountInit _msg)
        {
			NPPlayer.instance.sevenDayLoginComp.retSevenDayLoginInit(_msg);
        }
    }
}