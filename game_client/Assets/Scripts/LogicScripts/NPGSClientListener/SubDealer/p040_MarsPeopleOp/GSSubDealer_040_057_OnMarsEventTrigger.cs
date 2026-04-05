using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 事件触发 推送
    /// </summary>
    public class GSSubDealer_040_057_OnMarsEventTrigger : NPSubDealer<GS2GC_040_057_OnMarsEventTrigger>
    {
        protected override GS2GC_040_057_OnMarsEventTrigger _createProtocolObj()
        {
            return new GS2GC_040_057_OnMarsEventTrigger();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_057_OnMarsEventTrigger _msg)
        {
            NPPlayer.instance.marsComp?.peopleSubComponent?.onMarsEventTrigger(_msg);
        }
    }
}
