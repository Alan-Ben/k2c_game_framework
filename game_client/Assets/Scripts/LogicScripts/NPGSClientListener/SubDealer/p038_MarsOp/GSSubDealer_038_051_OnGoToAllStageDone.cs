using ALBasicProtocolPack;
using GS2GC.p038_MarsOp;

namespace GOE
{
    /// <summary>
    /// 前往火星所有阶段完成
    /// </summary>
    public class GSSubDealer_038_051_OnGoToAllStageDone : NPSubDealer<GS2GC_038_051_OnGoToAllStageDone>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_038_051_OnGoToAllStageDone _createProtocolObj()
        {
            return new GS2GC_038_051_OnGoToAllStageDone();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_038_051_OnGoToAllStageDone _msg)
        {
			NPPlayer.instance.marsComp.goToSubComponent.onGoToAllStageDone();
        }
    }
}