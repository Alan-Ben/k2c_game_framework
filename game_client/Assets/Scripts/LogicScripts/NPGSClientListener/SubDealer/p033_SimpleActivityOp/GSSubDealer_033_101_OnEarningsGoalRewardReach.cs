using ALBasicProtocolPack;
using GS2GC.p033_SimpleActivityOp;

namespace GOE
{
    /// <summary>
    /// 赚速目标达成情况变更
    /// </summary>
    public class GSSubDealer_033_101_OnEarningsGoalRewardReach : NPSubDealer<GS2GC_033_101_OnEarningsGoalRewardReach>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_033_101_OnEarningsGoalRewardReach _createProtocolObj()
        {
            return new GS2GC_033_101_OnEarningsGoalRewardReach();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_033_101_OnEarningsGoalRewardReach _msg)
        {
            NPPlayer.instance.earningGoalComp.onEarningsGoalRewardReach(_msg);
        }
    }
}