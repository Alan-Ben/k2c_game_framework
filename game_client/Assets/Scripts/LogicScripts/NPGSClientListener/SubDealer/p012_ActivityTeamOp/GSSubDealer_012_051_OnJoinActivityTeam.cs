using ALBasicProtocolPack;
using GS2GC.p012_ActivityTeamOp;

namespace GOE
{
    /// <summary>
    /// 加入活动队伍
    /// </summary>
    public class GSSubDealer_012_051_OnJoinActivityTeam : NPSubDealer<GS2GC_012_051_OnJoinActivityTeam>
    {
        protected override GS2GC_012_051_OnJoinActivityTeam _createProtocolObj()
        {
            return new GS2GC_012_051_OnJoinActivityTeam();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_012_051_OnJoinActivityTeam _msg)
        {
            NPPlayer.instance.commonActivityComp.onJoinActivityTeam(_msg);
        }
    }
}