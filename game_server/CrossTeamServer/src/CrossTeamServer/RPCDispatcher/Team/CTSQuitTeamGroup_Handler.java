package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSQuitTeamGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：退出跨服队伍分组
 */
public class CTSQuitTeamGroup_Handler extends RpcRequestHandler<CTSQuitTeamGroup>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSQuitTeamGroup _rpc)
	{
        CrossGroupMgr.getInstance().removeUsId(_rpc.req().getGroupId(), _rpc.req().getUsId());

		_rpc.commit();
	}
}
