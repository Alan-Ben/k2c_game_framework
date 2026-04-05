package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSQuitTeam;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import CrossTeamServer.CrossTeam.CrossTeamMemberInfo;
import GS2GC.p012_ActivityTeamOp.GS2GC_012_053_OnActivityTeamMemberRemove;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：退出队伍
 */
public class CTSQuitTeam_Handler extends RpcRequestHandler<CTSQuitTeam>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSQuitTeam _rpc)
	{
        CrossGroup group = CrossGroupMgr.getInstance().lookupByTeamId(_rpc.req().getTeamId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }

        CrossTeamInfo team = group.getTeamMgr().lookup(_rpc.req().getTeamId());
        if(null == team)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_NOT_FOUND.getCode());
            return;
        }

        //队长不能退出队伍
        if(team.getMemberMgr().isLeader(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_LEADER_NOT_QUIT.getCode());
            return;
        }

        //退出队伍
        CrossTeamMemberInfo quitMember = team.getMemberMgr().removeMember(_rpc.req().getCid());
        if(null == quitMember)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_NOT_FOUND.getCode());
            return;
        }

		_rpc.commit();

        //同步给队伍其他成员有队伍成员退出队伍
        GS2GC_012_053_OnActivityTeamMemberRemove proto = new GS2GC_012_053_OnActivityTeamMemberRemove();
        proto.setTeamId(team.getTeamId());
        proto.setCid(_rpc.req().getCid());

        team.getMemberMgr().broadcastMsg(proto);
	}
}
