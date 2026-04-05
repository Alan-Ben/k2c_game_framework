package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildAddPlayerRequest_2C;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildAddPlayerRequest_2C_Handler extends _ATBasicUSRpc_Handler<GuildAddPlayerRequest_2C>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildAddPlayerRequest_2C _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			_usServer.getGuildMgr().getJoinRequestMgr().rpcAddLocalPlayerRequest(_rpc.req().getGuildId(), _rpc.req().getCid());

			_rpc.commit();
    	});
	}
}
