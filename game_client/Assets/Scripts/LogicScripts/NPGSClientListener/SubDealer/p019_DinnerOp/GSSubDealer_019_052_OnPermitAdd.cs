
using ALBasicProtocolPack;
using GOE;
using GS2GC.p019_DinnerOp;

namespace GOE
{
    /// <summary>
    /// 新增晚宴凭证推送
    /// </summary>
    public class GSSubDealer_019_052_OnPermitAdd : NPSubDealer<GS2GC_019_052_OnPermitAdd>
    {
        protected override GS2GC_019_052_OnPermitAdd _createProtocolObj()
        {
            return new GS2GC_019_052_OnPermitAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_019_052_OnPermitAdd _msg)
        {
            NPPlayer.instance.dinnerComp.OnPermitAdd(_msg);
        }
    }
}