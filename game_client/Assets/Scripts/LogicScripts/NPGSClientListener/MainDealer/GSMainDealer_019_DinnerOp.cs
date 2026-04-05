using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_019_DinnerOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_019_DinnerOp()
        : base(19, 70)
        {
			regDealer(new GSSubDealer_019_050_OnDinnerAdd());
			regDealer(new GSSubDealer_019_051_OnDinnerEnd());
            regDealer(new GSSubDealer_019_052_OnPermitAdd());
			regDealer(new GSSubDealer_019_053_OnJoinerAdd());
        }
    }
}