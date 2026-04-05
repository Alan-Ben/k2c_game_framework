package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSJoinTeam;
import Common.CrossTeamEnum.ENPCrossTeamJoinType;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import CrossTeamServer.CrossTeam.CrossTeamMemberInfo;
import GS2GC.p012_ActivityTeamOp.GS2GC_012_055_OnActivityTeamMemberAdd;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：加入队伍
 */
public class CTSJoinTeam_Handler extends RpcRequestHandler<CTSJoinTeam>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSJoinTeam _rpc)
	{
        //没有找到对应的分组
        CrossGroup group = CrossGroupMgr.getInstance().lookupByTeamId(_rpc.req().getTeamId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }


        if(group.getTeamMgr().isInTeam(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_EXISTED.getCode());
            return;
        }

        CrossTeamInfo team = group.getTeamMgr().lookup(_rpc.req().getTeamId());
        if(null == team)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_NOT_FOUND.getCode());
            return;
        }

        if(team.getMemberMgr().isFull())
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_FULL.getCode());
            return;
        }

        // 只有自由加入模式才允许直接加入
        if(team.getJoinType() != ENPCrossTeamJoinType.FREE_JOIN)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_NOT_FREE_JOIN.getCode());
            return;
        }

        // 检查加入条件
        if(!team.checkJoinCond(_rpc.req().getJoinCondList()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_APPLY_COND_FAIL.getCode());
            return;
        }

        CrossTeamMemberInfo member = team.getMemberMgr().addMember(_rpc.req().getCid());
        if(null == member)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_JOIN_FAIL.getCode());
            return;
        }

        _rpc.retObj().setTeam(team.toProto());
		_rpc.commit();


        //广播同步队伍信息
        GS2GC_012_055_OnActivityTeamMemberAdd proto = new GS2GC_012_055_OnActivityTeamMemberAdd();
        proto.setTeamId(team.getTeamId());
        proto.setMember(member.toProto());

        team.getMemberMgr().broadcastMsg(proto, _rpc.req().getCid());
	}
}
