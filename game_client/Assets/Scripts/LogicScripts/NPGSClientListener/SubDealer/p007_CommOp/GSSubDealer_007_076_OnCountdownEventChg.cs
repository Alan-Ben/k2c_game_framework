using ALBasicProtocolPack;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 倒计时事件变更
    /// </summary>
    public class GSSubDealer_007_076_OnCountdownEventChg : NPSubDealer<GS2GC_007_076_OnCountdownEventChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_076_OnCountdownEventChg _createProtocolObj()
        {
            return new GS2GC_007_076_OnCountdownEventChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_076_OnCountdownEventChg _msg)
        {
			NPPlayer.instance.countdownEventComp.onCountdownEventChg(_msg);
        }
    }
}