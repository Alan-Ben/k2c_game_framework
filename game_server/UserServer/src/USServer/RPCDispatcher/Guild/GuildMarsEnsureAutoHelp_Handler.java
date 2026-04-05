package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMarsEnsureAutoHelp;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildMarsEnsureAutoHelp_Handler extends _ATBasicUSRpc_Handler<GuildMarsEnsureAutoHelp>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMarsEnsureAutoHelp _rpc)
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

			//添加玩家自动互助
			guildInfo.getMarsHelpMgr().getAutoHelpPlayerMgr().ensure(_rpc.req().getCid(), _rpc.req().getEndTimeMS());
    		
    		_rpc.commit();
    	});
	}
}
