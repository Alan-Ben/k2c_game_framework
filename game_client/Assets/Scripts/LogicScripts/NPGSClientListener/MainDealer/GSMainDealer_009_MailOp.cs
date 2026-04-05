using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_009_MailOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_009_MailOp() :
            base(9, 70)
        {
            regDealer(new GSSubDealer_009_051_OnMailAdded());
            regDealer(new GSSubDealer_009_052_OnMailRemoved());
            regDealer(new GSSubDealer_009_053_OnMailLockedUpdated());
            regDealer(new GSSubDealer_009_054_OnMailReaded());
            regDealer(new GSSubDealer_009_055_OnMailRewardTaken());
            regDealer(new GSSubDealer_009_056_OnMailExDataUpdated());
            regDealer(new GSSubDealer_009_057_OnMailReadOver());
            regDealer(new GSSubDealer_009_058_OnMailExpiredSecChg());
        }
    }
}
