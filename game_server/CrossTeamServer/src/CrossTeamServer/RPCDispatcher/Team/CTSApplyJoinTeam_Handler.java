package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSApplyJoinTeam;
import Common.CrossTeamEnum.ENPCrossTeamJoinType;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：申请加入队伍
 */
public class CTSApplyJoinTeam_Handler extends RpcRequestHandler<CTSApplyJoinTeam>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSApplyJoinTeam _rpc)
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

        //检查玩家是否已在队伍中
        if(group.getTeamMgr().isInTeam(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_EXISTED.getCode());
            return;
        }

        //禁止加入模式下不允许提交申请
        if(team.getJoinType() == ENPCrossTeamJoinType.FORBID_JOIN)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_FORBID_JOIN.getCode());
            return;
        }

        //检查玩家是否已申请过该队伍
        if(null != team.getApplyMgr().lookupByCid(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_APPLY_ALREADY_EXIST.getCode());
            return;
        }

        //检查申请条件
        if(!team.checkJoinCond(_rpc.req().getJoinCondList()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_APPLY_COND_FAIL.getCode());
            return;
        }

        team.getApplyMgr().addPlayerApply(_rpc.req().getCid());

		_rpc.commit();
	}
}
