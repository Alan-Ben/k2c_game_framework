using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 移除活动推送
    /// </summary>
    public class GSSubDealer_017_052_OnActivityRemove : NPSubDealer<GS2GC_017_052_OnActivityRemove>
    {
        protected override GS2GC_017_052_OnActivityRemove _createProtocolObj()
        {
            return new GS2GC_017_052_OnActivityRemove();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_052_OnActivityRemove _msg)
        {
            NPPlayer.instance.commonActivityComp.onActivityRemove(_msg);
        }
    }
}