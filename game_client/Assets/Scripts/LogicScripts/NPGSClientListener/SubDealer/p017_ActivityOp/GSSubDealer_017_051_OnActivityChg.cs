using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 活动数据变化推送
    /// </summary>
    public class GSSubDealer_017_051_OnActivityChg : NPSubDealer<GS2GC_017_051_OnActivityChg>
    {
        protected override GS2GC_017_051_OnActivityChg _createProtocolObj()
        {
            return new GS2GC_017_051_OnActivityChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_051_OnActivityChg _msg)
        {
            NPPlayer.instance.commonActivityComp.onActivityChg(_msg);
        }
    }
}