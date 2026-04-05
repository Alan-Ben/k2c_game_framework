package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSGetGLSTeamData;
import Common.ServerObj.ServerObj_FirstTeam_Team;
import Common.ServerObj.ServerObj_GLSTeamMember;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;

/*******
 * 处理逻辑：获取 GLS GroupMgr 所需的指定队伍数据
 * 返回队长 cid 和成员 cid 列表，供 GLS 构建本地队伍缓存
 */
public class CTSGetGLSTeamData_Handler extends RpcRequestHandler<CTSGetGLSTeamData>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSGetGLSTeamData _rpc)
	{
		long teamId = _rpc.req().getTeamId();

        // 通过 teamId 找到所属分组
        CrossGroup group = CrossGroupMgr.getInstance().lookupByTeamId(teamId);
        if (null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }

        // 找到具体队伍
        CrossTeamInfo team = group.getTeamMgr().lookup(teamId);
        if (null == team)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_NOT_FOUND.getCode());
            return;
        }

        // 构造 GLS 所需队伍数据
        ArrayList<ServerObj_GLSTeamMember> memberList = new ArrayList<>();
        team.getMemberMgr().makeGLSMemberList(memberList);

        ServerObj_FirstTeam_Team teamData = new ServerObj_FirstTeam_Team(
                team.getMemberMgr().getLeaderCid(), memberList);

        _rpc.retObj().setTeamData(teamData);
        _rpc.commit();
	}
}
