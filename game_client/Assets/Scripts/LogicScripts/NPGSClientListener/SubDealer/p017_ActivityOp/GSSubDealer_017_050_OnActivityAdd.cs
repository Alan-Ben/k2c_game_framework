using ALBasicProtocolPack;
using GS2GC.p017_ActivityOp;

namespace GOE
{
    /// <summary>
    /// 新增活动推送
    /// </summary>
    public class GSSubDealer_017_050_OnActivityAdd : NPSubDealer<GS2GC_017_050_OnActivityAdd>
    {
        protected override GS2GC_017_050_OnActivityAdd _createProtocolObj()
        {
            return new GS2GC_017_050_OnActivityAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_017_050_OnActivityAdd _msg)
        {
            NPPlayer.instance.commonActivityComp.onActivityAdd(_msg);
        }
    }
}