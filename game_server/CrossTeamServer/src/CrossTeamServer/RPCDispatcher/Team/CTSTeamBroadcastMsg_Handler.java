package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSTeamBroadcastMsg;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：队伍成员广播消息
 */
public class CTSTeamBroadcastMsg_Handler extends RpcRequestHandler<CTSTeamBroadcastMsg>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSTeamBroadcastMsg _rpc)
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

        team.getMemberMgr().broadcastMsg(_rpc.req().get_buffer_Protocol(), 0);

		_rpc.commit();
	}
}
