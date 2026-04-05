using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 删除求助 推送
    /// </summary>
    public class GSSubDealer_040_056_OnHelpDel : NPSubDealer<GS2GC_040_056_OnHelpDel>
    {
        protected override GS2GC_040_056_OnHelpDel _createProtocolObj()
        {
            return new GS2GC_040_056_OnHelpDel();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_056_OnHelpDel _msg)
        {
            NPPlayer.instance.marsComp?.peopleSubComponent?.onMarsHelpDel(_msg);
        }
    }
}
