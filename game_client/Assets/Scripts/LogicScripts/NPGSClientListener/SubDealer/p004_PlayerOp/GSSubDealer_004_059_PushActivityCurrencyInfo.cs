using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 活动货币信息推送
    /// </summary>
    public class GSSubDealer_004_059_PushActivityCurrencyInfo : NPSubDealer<GS2GC_004_059_PushActivityCurrencyInfo>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_059_PushActivityCurrencyInfo _createProtocolObj()
        {
            return new GS2GC_004_059_PushActivityCurrencyInfo();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_059_PushActivityCurrencyInfo _msg)
        {
			NPPlayer.instance.commonActivityComp.onActivityCurrencyInfoChg(_msg);
        }
    }
}