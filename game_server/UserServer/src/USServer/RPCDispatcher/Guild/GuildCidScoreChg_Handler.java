package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildOnScoreChg;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildCidScoreChg_Handler extends _ATBasicUSRpc_Handler<GuildOnScoreChg>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildOnScoreChg _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			_usServer.getRankListMgr().rpcOnGuildScoreChg(_rpc.req().getRankInstanceId(), _rpc.req().getCid(), _rpc.req().getGuildId(), _rpc.req().getScoreSourceId(), _rpc.req().getChgValue());

			_rpc.commit();
    	});
	}
}
