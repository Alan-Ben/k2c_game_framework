package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSKickTeamPlayer;
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
 * 处理逻辑：踢出队伍成员
 */
public class CTSKickTeamPlayer_Handler extends RpcRequestHandler<CTSKickTeamPlayer>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSKickTeamPlayer _rpc)
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

        //执行踢出队伍成员
        CrossTeamMemberInfo kickMember = team.getMemberMgr().removeMember(_rpc.req().getKickCid());
        if(null == kickMember)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_MEMBER_NOT_FOUND.getCode());
            return;
        }

        _rpc.commit();

        //对被踢出玩家的消息推送
        kickMember.sendQuitTeamMsg();

        //通知队伍其他成员有队伍成员退出队伍
        GS2GC_012_053_OnActivityTeamMemberRemove proto = new GS2GC_012_053_OnActivityTeamMemberRemove();
        proto.setTeamId(team.getTeamId());
        proto.setCid(_rpc.req().getKickCid());

        team.getMemberMgr().broadcastMsg(proto);
	}
}
