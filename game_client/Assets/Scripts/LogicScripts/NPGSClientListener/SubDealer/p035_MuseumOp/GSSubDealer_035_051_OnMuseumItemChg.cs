using ALBasicProtocolPack;
using GS2GC.p035_MuseumOp;

namespace GOE
{
    /// <summary>
    /// 博物馆物品新增推送
    /// </summary>
    public class GSSubDealer_035_051_OnMuseumItemChg : NPSubDealer<GS2GC_035_051_OnMuseumItemChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_035_051_OnMuseumItemChg _createProtocolObj()
        {
            return new GS2GC_035_051_OnMuseumItemChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_035_051_OnMuseumItemChg _msg)
        {
			NPPlayer.instance.museumComp._onMuseumItemChg(_msg);
        }
    }
}