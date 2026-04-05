using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 活动状态变更推送
    /// </summary>
    public class GSSubDealer_017_056_OnActivityStateChg : NPSubDealer<GS2GC_017_056_OnActivityStateChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_017_056_OnActivityStateChg _createProtocolObj()
        {
            return new GS2GC_017_056_OnActivityStateChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_056_OnActivityStateChg _msg)
        {
            NPPlayer.instance.commonActivityComp.onActivityStateChg(_msg);
        }
    }
}