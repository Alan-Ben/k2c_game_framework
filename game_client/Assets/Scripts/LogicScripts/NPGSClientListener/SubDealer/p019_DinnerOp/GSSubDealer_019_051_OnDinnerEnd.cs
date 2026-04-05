using ALBasicProtocolPack;
using GS2GC.p019_DinnerOp;

namespace GOE
{
    /// <summary>
    /// 晚宴结束推送
    /// </summary>
    public class GSSubDealer_019_051_OnDinnerEnd : NPSubDealer<GS2GC_019_051_OnDinnerEnd>
    {
        protected override GS2GC_019_051_OnDinnerEnd _createProtocolObj()
        {
            return new GS2GC_019_051_OnDinnerEnd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_019_051_OnDinnerEnd _msg)
        {
            NPPlayer.instance.dinnerComp.OnDinnerEnd(_msg);
        }
    }
}