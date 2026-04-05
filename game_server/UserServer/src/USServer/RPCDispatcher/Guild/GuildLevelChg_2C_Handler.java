package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildLevelChg_2C;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildLevelChg_2C_Handler extends _ATBasicUSRpc_Handler<GuildLevelChg_2C>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildLevelChg_2C _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_rpc.req().getCid());
			if(null == userData) {
				_rpc.commit();
				return;
			}

			//玩家有数据对象则进行相关处理
			userData.safeCall(() ->
			{
				userData.getGuildComponent().onGuildLevelChg(_rpc.req().getGuildId(), _rpc.req().getGuildLvl());
			});

			_rpc.commit();
    	});
	}
}
