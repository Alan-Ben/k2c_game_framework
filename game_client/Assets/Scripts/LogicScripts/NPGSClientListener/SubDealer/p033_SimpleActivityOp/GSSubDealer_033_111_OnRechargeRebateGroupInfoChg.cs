using ALBasicProtocolPack;
using GS2GC.p033_SimpleActivityOp;

namespace GOE
{
    /// <summary>
    /// 充值返利组信息变化
    /// </summary>
    public class GSSubDealer_033_111_OnRechargeRebateGroupInfoChg : NPSubDealer<GS2GC_033_111_OnRechargeRebateGroupInfoChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_033_111_OnRechargeRebateGroupInfoChg _createProtocolObj()
        {
            return new GS2GC_033_111_OnRechargeRebateGroupInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_033_111_OnRechargeRebateGroupInfoChg _msg)
        {
            NPPlayer.instance.rechargeRebateComp.onRechargeRebateGroupInfoChg(_msg);
        }
    }
}