package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSRegUsGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 处理逻辑：注册US到跨服队伍分组
 */
public class CTSRegUsGroup_Handler extends RpcRequestHandler<CTSRegUsGroup> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSRegUsGroup _rpc)
    {
        // 确保分组存在并注册US服务器ID
        CrossGroupMgr.getInstance().ensure(_rpc.req().getGroupId(), _rpc.req().getUsId());

        _rpc.commit();
    }
}
