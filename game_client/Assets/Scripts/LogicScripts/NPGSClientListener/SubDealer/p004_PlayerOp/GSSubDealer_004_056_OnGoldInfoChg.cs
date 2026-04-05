using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_004_056_OnGoldInfoChg : NPSubDealer<GS2GC_004_056_OnGoldInfoChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_056_OnGoldInfoChg _createProtocolObj()
        {
            return new GS2GC_004_056_OnGoldInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_056_OnGoldInfoChg _msg)
        {
            NPPlayer.instance.specialItemComp.goldData._onGoldInfoChg(_msg);
        }
    }
}