using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 新增或变化信件推送
    /// </summary>
    public class GSSubDealer_040_053_OnLetterChg : NPSubDealer<GS2GC_040_053_OnLetterChg>
    {
        protected override GS2GC_040_053_OnLetterChg _createProtocolObj()
        {
            return new GS2GC_040_053_OnLetterChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_053_OnLetterChg _msg)
        {
            NPPlayer.instance.marsComp?.peopleSubComponent?.onMarsLetterChg(_msg);
        }
    }
}
