using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_042_GuildRelatedOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_042_GuildRelatedOp()
        : base(42, 70)
        {
            regDealer(new GSSubDealer_042_050_OnMyMarsHelpChg());
			regDealer(new GSSubDealer_042_051_OnCanDealMarsHelpAdd());
			regDealer(new GSSubDealer_042_052_OnCanDealMarsHelpDel());
			regDealer(new GSSubDealer_042_053_OnMyMarsHelpDealed());
			regDealer(new GSSubDealer_042_054_OnMyMarsHelpDel());
			regDealer(new GSSubDealer_042_055_OnGuildBoxAddCountChg());
			regDealer(new GSSubDealer_042_056_OnGuildBoxRewardShow());
			regDealer(new GSSubDealer_042_057_OnGuildBoxAdd());
			regDealer(new GSSubDealer_042_058_OnGuildActivePointChg());
        }
    }
}
