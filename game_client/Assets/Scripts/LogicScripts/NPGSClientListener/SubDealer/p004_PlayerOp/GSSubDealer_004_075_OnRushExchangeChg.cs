using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 急速兑换信息变更推送
    /// </summary>
    public class GSSubDealer_004_075_OnRushExchangeChg : NPSubDealer<GS2GC_004_075_OnRushExchangeChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_075_OnRushExchangeChg _createProtocolObj()
        {
            return new GS2GC_004_075_OnRushExchangeChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_075_OnRushExchangeChg _msg)
        {
            NPPlayer.instance.rushExchangeComp.onRushExchangeChg(_msg);
        }
    }
}
