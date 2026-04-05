using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_024_DungeonOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_024_DungeonOp()
        : base(24, 70)
        {
			regDealer(new GSSubDealer_024_052_OnMiddayDungeonTimeInfoChg());
        	regDealer(new GSSubDealer_024_051_OnMiddayDungeonInfoChg());
        	
            regDealer(new GSSubDealer_024_061_OnEveningDungeonInfoChg());
            regDealer(new GSSubDealer_024_062_OnEveningDungeonTimeInfoChg());
        }
    }
}