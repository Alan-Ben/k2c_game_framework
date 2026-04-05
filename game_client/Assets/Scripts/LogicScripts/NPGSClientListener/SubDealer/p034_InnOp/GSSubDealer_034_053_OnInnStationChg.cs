using ALBasicProtocolPack;
using GS2GC.p034_InnOp;

namespace GOE
{
    /// <summary>
    /// 旅店设施信息变更
    /// </summary>
    public class GSSubDealer_034_053_OnInnStationChg : NPSubDealer<GS2GC_034_053_OnInnStationChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_034_053_OnInnStationChg _createProtocolObj()
        {
            return new GS2GC_034_053_OnInnStationChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_034_053_OnInnStationChg _msg)
        {
			NPPlayer.instance.innComp._onInnStationChg(_msg);
        }
    }
}