using ALBasicProtocolPack;
using GS2GC.p034_InnOp;

namespace GOE
{
    /// <summary>
    /// 旅店特殊客人变更
    /// </summary>
    public class GSSubDealer_034_061_OnInnSpecialGuestChg : NPSubDealer<GS2GC_034_061_OnInnSpecialGuestChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_034_061_OnInnSpecialGuestChg _createProtocolObj()
        {
            return new GS2GC_034_061_OnInnSpecialGuestChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_034_061_OnInnSpecialGuestChg _msg)
        {
            NPPlayer.instance.innComp._onInnSpecialGuestHandbookChg(_msg);
        }
    }
}