using ALBasicProtocolPack;
using GS2GC.p019_DinnerOp;

namespace GOE
{
    /// <summary>
    /// 新增晚宴推送
    /// </summary>
    public class GSSubDealer_019_050_OnDinnerAdd : NPSubDealer<GS2GC_019_050_OnDinnerAdd>
    {
        protected override GS2GC_019_050_OnDinnerAdd _createProtocolObj()
        {
            return new GS2GC_019_050_OnDinnerAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_019_050_OnDinnerAdd _msg)
        {
            NPPlayer.instance.dinnerComp.OnDinnerAdd(_msg);
        }
    }
}