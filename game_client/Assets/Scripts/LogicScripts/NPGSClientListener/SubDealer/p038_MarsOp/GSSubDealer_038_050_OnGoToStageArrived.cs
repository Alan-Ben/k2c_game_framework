using ALBasicProtocolPack;
using GS2GC.p038_MarsOp;

namespace GOE
{
    /// <summary>
    /// 前往火星-到达新阶段
    /// </summary>
    public class GSSubDealer_038_050_OnGoToStageArrived : NPSubDealer<GS2GC_038_050_OnGoToStageArrived>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_038_050_OnGoToStageArrived _createProtocolObj()
        {
            return new GS2GC_038_050_OnGoToStageArrived();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_038_050_OnGoToStageArrived _msg)
        {
			NPPlayer.instance.marsComp.goToSubComponent.onGoToStageArrived(_msg);
        }
    }
}