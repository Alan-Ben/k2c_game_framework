using ALBasicProtocolPack;
using GS2GC.p033_SimpleActivityOp;

namespace GOE
{
    /// <summary>
    /// 充值返利奖励领取变更
    /// </summary>
    public class GSSubDealer_033_110_OnRechargeRebateRewardDraw : NPSubDealer<GS2GC_033_110_OnRechargeRebateRewardDraw>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_033_110_OnRechargeRebateRewardDraw _createProtocolObj()
        {
            return new GS2GC_033_110_OnRechargeRebateRewardDraw();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_033_110_OnRechargeRebateRewardDraw _msg)
        {
			NPPlayer.instance.rechargeRebateComp.onRechargeRebateRewardDraw(_msg);
        }
    }
}