using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 活动热更配表信息变更推送
    /// </summary>
    public class GSSubDealer_017_062_OnActivityHotRefInfoChg : NPSubDealer<GS2GC_017_062_OnActivityHotRefInfoChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_017_062_OnActivityHotRefInfoChg _createProtocolObj()
        {
            return new GS2GC_017_062_OnActivityHotRefInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_062_OnActivityHotRefInfoChg _msg)
        {
			NPPlayer.instance.commonActivityHotRefComp.onActivityHotRefInfoChg(_msg);
        }
    }
}