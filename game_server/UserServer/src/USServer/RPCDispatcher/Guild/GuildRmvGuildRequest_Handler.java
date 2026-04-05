package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildRmvGuildRequest;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildRmvGuildRequest_Handler extends _ATBasicUSRpc_Handler<GuildRmvGuildRequest>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildRmvGuildRequest _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			_usServer.getGuildMgr().getJoinRequestMgr().rpcRmvLocalGuildRequest(_rpc.req().getGuildId(), _rpc.req().getCid());

			_rpc.commit();
    	});
	}
}
