using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 活动基金-新增推送
    /// </summary>
    public class GSSubDealer_017_067_OnActivityFundAdd : NPSubDealer<GS2GC_017_067_OnActivityFundAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_017_067_OnActivityFundAdd _createProtocolObj()
        {
            return new GS2GC_017_067_OnActivityFundAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_067_OnActivityFundAdd _msg)
        {
            NPPlayer.instance.fundComp._onFundAdd(_msg);
        }
    }
}