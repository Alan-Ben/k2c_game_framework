package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildAddMarsMine;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildAddMarsMine_Handler extends _ATBasicUSRpc_Handler<GuildAddMarsMine>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildAddMarsMine _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			//获取公会信息
			GuildInfo guildInfo = _usServer.getGuildMgr().lookupGuild(_rpc.req().getGuildId());
			if(null == guildInfo)
			{
				_rpc.commitFail(GuildErr.GUILD_NOT_EXIST.getCode());
				return ;
			}

			guildInfo.getMarsMineShareMgr().addMineShare(_rpc.req().getCid(), _rpc.req().getMineId(), _rpc.req().getPosId(), _rpc.req().getEndShowTimeMS());

			_rpc.commit();
    	});
	}
}
