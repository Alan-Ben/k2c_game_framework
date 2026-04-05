package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSAgreeTeamApply;
import CTSDB.Bo.CrossTeamApplyBO;
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
 * 处理逻辑：同意玩家入队申请
 */
public class CTSAgreeTeamApply_Handler extends RpcRequestHandler<CTSAgreeTeamApply>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSAgreeTeamApply _rpc)
	{
        //没有找到对应的分组
        CrossGroup group = CrossGroupMgr.getInstance().lookupByTeamId(_rpc.req().getTeamId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }

        //检查队伍是否存在
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

        //检查权限
        if(!team.getMemberMgr().isLeader(_rpc.req().getCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_NOT_LEADER.getCode());
            return;
        }

        //检查申请数据
        CrossTeamApplyBO bo = team.getApplyMgr().removePlayerApply(_rpc.req().getApplyCid());
        if(null == bo)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_APPLY_NOT_FOUND.getCode());
            return;
        }

        //检查玩家是否已经在队伍中
        if (group.getTeamMgr().isInTeam(bo.getApplyCid()))
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_EXISTED.getCode());
            return;
        }

        //同意申请，添加玩家到队伍中
        CrossTeamMemberInfo member = team.getMemberMgr().addMember(bo.getApplyCid());
        if(null == member)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_JOIN_FAIL.getCode());
            return;
        }

		_rpc.commit();

        //广播同步队伍信息
        GS2GC_012_055_OnActivityTeamMemberAdd proto = new GS2GC_012_055_OnActivityTeamMemberAdd();
        proto.setTeamId(team.getTeamId());
        proto.setMember(member.toProto());

        team.getMemberMgr().broadcastMsg(proto, 0);
	}
}
