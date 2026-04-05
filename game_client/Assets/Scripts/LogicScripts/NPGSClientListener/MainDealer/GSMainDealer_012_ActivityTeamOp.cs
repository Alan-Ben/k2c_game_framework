using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_012_ActivityTeamOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_012_ActivityTeamOp()
        : base(12, 70)
        {
            regDealer(new GSSubDealer_012_051_OnJoinActivityTeam());
            regDealer(new GSSubDealer_012_052_OnQuitActivityTeam());
			regDealer(new GSSubDealer_012_053_OnActivityTeamMemberRemove());
			regDealer(new GSSubDealer_012_054_OnActivityTeamDissolve());
			regDealer(new GSSubDealer_012_055_OnActivityTeamMemberAdd());
        }
    }
}