using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 满意度变化推送
    /// </summary>
    public class GSSubDealer_040_052_OnSatisfactionChg : NPSubDealer<GS2GC_040_052_OnSatisfactionChg>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_040_052_OnSatisfactionChg _createProtocolObj()
        {
            return new GS2GC_040_052_OnSatisfactionChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_052_OnSatisfactionChg _msg)
        {
            NPPlayer.instance.marsComp?.peopleSubComponent?.onMarsSatisfactionChg(_msg);
        }
    }
}
