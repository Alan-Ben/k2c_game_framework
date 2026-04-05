using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 订单信息变更
    /// </summary>
    public class GSSubDealer_004_067_OnOrderChg : NPSubDealer<GS2GC_004_067_OnOrderChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_067_OnOrderChg _createProtocolObj()
        {
            return new GS2GC_004_067_OnOrderChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_067_OnOrderChg _msg)
        {
			NPPlayer.instance.payOrderComp.onOrderChg(_msg);
        }
    }
}