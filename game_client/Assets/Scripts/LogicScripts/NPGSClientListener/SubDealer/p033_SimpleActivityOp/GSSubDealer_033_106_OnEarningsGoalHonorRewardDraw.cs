using ALBasicProtocolPack;
using GS2GC.p033_SimpleActivityOp;

namespace GOE
{
    /// <summary>
    /// 赚速目标荣耀领取情况变更
    /// </summary>
    public class GSSubDealer_033_106_OnEarningsGoalHonorRewardDraw : NPSubDealer<GS2GC_033_106_OnEarningsGoalHonorRewardDraw>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_033_106_OnEarningsGoalHonorRewardDraw _createProtocolObj()
        {
            return new GS2GC_033_106_OnEarningsGoalHonorRewardDraw();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_033_106_OnEarningsGoalHonorRewardDraw _msg)
        {
			NPPlayer.instance.earningGoalComp.onEarningsGoalHonorRewardDraw(_msg);
        }
    }
}