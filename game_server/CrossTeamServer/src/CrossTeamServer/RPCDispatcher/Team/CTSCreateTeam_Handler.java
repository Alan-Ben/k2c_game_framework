package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSCreateTeam;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 处理逻辑：创建队伍
 */
public class CTSCreateTeam_Handler extends RpcRequestHandler<CTSCreateTeam> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSCreateTeam _rpc)
    {
        //获取分组数据（分组已由US启动时的_regUs注册，此处直接查找）
        CrossGroup group = CrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }
        //检查玩家是否已经在队伍中
        if (group.getTeamMgr().isInTeam(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_EXISTED.getCode());
            return;
        }

        //创建队伍
        CrossTeamInfo team = group.getTeamMgr().create(group.getCurTeamIdx(), _rpc.req().getCid(),
                _rpc.req().getMemberLimit(),
                _rpc.req().getJoinType(), _rpc.req().getJoinCond(),
                _rpc.req().getTeamName(), _rpc.req().getTeamDec());
        if (null == team)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_CREATE_FAIL.getCode());
            return;
        }

        _rpc.retObj().setTeam(team.toProto());
		_rpc.commit();
	}
}
