using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_008_TravelOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_008_TravelOp()
        : base(8, 70)
        {
            regDealer(new GSSubDealer_008_050_OnEventAdd());
			regDealer(new GSSubDealer_008_051_OnEventDel());
			regDealer(new GSSubDealer_008_052_OnConsortChg());
        }
    }
}