package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSDissolveTeam;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：解散跨服队伍
 */
public class CTSDissolveTeam_Handler extends RpcRequestHandler<CTSDissolveTeam>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSDissolveTeam _rpc)
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

        //执行解散队伍
        group.getTeamMgr().dissolve(team.getTeamId());

        //执行解散前先通知 GLS 移除 Group（此时成员数据仍完整，队长 cid 可访问）
        team.notifyGroupRemove();

		_rpc.commit();
	}
}
