package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSSetTeamSetting;
import Common.CrossTeamEnum.ENPCrossTeamJoinCond;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑：修改队伍设置（名称、宣言、加入方式、申请条件）
 */
public class CTSSetTeamSetting_Handler extends RpcRequestHandler<CTSSetTeamSetting>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSSetTeamSetting _rpc)
	{
		CrossGroup group = CrossGroupMgr.getInstance().lookupByTeamId(_rpc.req().getTeamId());
		if (null == group)
		{
			_rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
			return;
		}

		CrossTeamInfo team = group.getTeamMgr().lookup(_rpc.req().getTeamId());
		if (null == team)
		{
			_rpc.commitFail(CrossTeamErr.TEAM_NOT_FOUND.getCode());
			return;
		}

		// 只有队长才能修改队伍设置
		if (!team.getMemberMgr().isLeader(_rpc.req().getCid()))
		{
			_rpc.commitFail(CrossTeamErr.TEAM_MEMBER_NOT_LEADER.getCode());
			return;
		}

		// 条件加入模式时申请条件类型不能为NONE
		if (_rpc.req().getJoinCond().getCond() == ENPCrossTeamJoinCond.NONE
				&& team.getJoinCond().getCond() != ENPCrossTeamJoinCond.NONE)
		{
			_rpc.commitFail(CrossTeamErr.TEAM_APPLY_COND_ERROR.getCode());
			return;
		}

		team.setTeamSetting(_rpc.req().getTeamName(), _rpc.req().getTeamDec(),
				_rpc.req().getJoinType(), _rpc.req().getJoinCond());

		_rpc.commit();
	}
}
