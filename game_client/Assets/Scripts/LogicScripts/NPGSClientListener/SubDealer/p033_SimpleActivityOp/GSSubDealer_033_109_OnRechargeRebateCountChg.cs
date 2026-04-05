using ALBasicProtocolPack;
using GS2GC.p033_SimpleActivityOp;

namespace GOE
{
    /// <summary>
    /// 充值返利计数变化
    /// </summary>
    public class GSSubDealer_033_109_OnRechargeRebateCountChg : NPSubDealer<GS2GC_033_109_OnRechargeRebateCountChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_033_109_OnRechargeRebateCountChg _createProtocolObj()
        {
            return new GS2GC_033_109_OnRechargeRebateCountChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_033_109_OnRechargeRebateCountChg _msg)
        {
			NPPlayer.instance.rechargeRebateComp.onRechargeRebateCountChg(_msg);
        }
    }
}