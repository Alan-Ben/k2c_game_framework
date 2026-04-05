using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 阶段奖励积分变化
    /// </summary>
    public class GSSubDealer_017_057_OnActivityStepRewardScoreChg : NPSubDealer<GS2GC_017_057_OnActivityStepRewardScoreChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_017_057_OnActivityStepRewardScoreChg _createProtocolObj()
        {
            return new GS2GC_017_057_OnActivityStepRewardScoreChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_057_OnActivityStepRewardScoreChg _msg)
        {
			NPPlayer.instance.commonActivityComp.onActivityStepRewardScoreChg(_msg);
        }
    }
}