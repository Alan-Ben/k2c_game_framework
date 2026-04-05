using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 决策数据变化推送
    /// </summary>
    public class GSSubDealer_040_051_OnIntelligentChg : NPSubDealer<GS2GC_040_051_OnIntelligentChg>
    {
        protected override GS2GC_040_051_OnIntelligentChg _createProtocolObj()
        {
            return new GS2GC_040_051_OnIntelligentChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_051_OnIntelligentChg _msg)
        {
            NPPlayer.instance.marsComp.peopleSubComponent.onMarsIntelligentChg(_msg);
        }
    }
}
