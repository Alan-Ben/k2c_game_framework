package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildSimpleNameChg_2C;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildSimpleNameChg_2C_Handler extends _ATBasicUSRpc_Handler<GuildSimpleNameChg_2C>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildSimpleNameChg_2C _rpc)
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
				userData.getGuildComponent().onGuildSimpleNameChg(_rpc.req().getGuildId(), _rpc.req().getGuildName(), _rpc.req().getSimpleName());
			});

			_rpc.commit();
    	});
	}
}
