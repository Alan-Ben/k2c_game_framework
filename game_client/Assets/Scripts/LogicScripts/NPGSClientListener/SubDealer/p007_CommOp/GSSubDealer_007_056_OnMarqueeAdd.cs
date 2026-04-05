using ALBasicProtocolPack;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 新增跑马灯
    /// </summary>
    public class GSSubDealer_007_056_OnMarqueeAdd : NPSubDealer<GS2GC_007_056_OnMarqueeAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_056_OnMarqueeAdd _createProtocolObj()
        {
            return new GS2GC_007_056_OnMarqueeAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_056_OnMarqueeAdd _msg)
        {
            NPPlayer.instance.marqueeComp.onMarqueeAdd(_msg);
        }
    }
}