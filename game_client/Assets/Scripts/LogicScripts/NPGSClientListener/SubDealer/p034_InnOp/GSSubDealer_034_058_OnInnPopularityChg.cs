using ALBasicProtocolPack;
using GS2GC.p034_InnOp;

namespace GOE
{
    /// <summary>
    /// 旅店人气值变更
    /// </summary>
    public class GSSubDealer_034_058_OnInnPopularityChg : NPSubDealer<GS2GC_034_058_OnInnPopularityChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_034_058_OnInnPopularityChg _createProtocolObj()
        {
            return new GS2GC_034_058_OnInnPopularityChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_034_058_OnInnPopularityChg _msg)
        {
			NPPlayer.instance.innComp._onInnPopularityChg(_msg);
        }
    }
}