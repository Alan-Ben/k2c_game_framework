package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSSetTeamApplyCond;
import Common.CrossTeamEnum.ENPCrossTeamJoinCond;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：设置队伍申请条件
 */
public class CTSSetTeamApplyCond_Handler extends RpcRequestHandler<CTSSetTeamApplyCond>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSSetTeamApplyCond _rpc)
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

        //检查权限
        if(!team.getMemberMgr().isLeader(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_NOT_LEADER.getCode());
            return;
        }

        //检查申请条件类型：条件类型必须匹配且不为NONE
        if(team.getJoinCond().getCond() == ENPCrossTeamJoinCond.NONE
            || team.getJoinCond().getCond() != _rpc.req().getCondValue().getCond())
        {
            _rpc.commitFail(CrossTeamErr.TEAM_APPLY_COND_ERROR.getCode());
            return;
        }

        team.updateJoinCond(_rpc.req().getCondValue());

		_rpc.commit();
	}
}
