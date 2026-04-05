package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMemberRmv_2C;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildMemberRmv_2C_Handler extends _ATBasicUSRpc_Handler<GuildMemberRmv_2C>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMemberRmv_2C _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			OfflineRewardFunc.onPlayerQuitGuild(_usServer, _rpc.req().getCid(), _rpc.req().getGuildId(), _rpc.req().getGuildName(), _rpc.req().getTimeMS(), _rpc.req().getIsKicked());

			_rpc.commit();
    	});
	}
}
