package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildRequestLoadRankData;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildRequestLoadRankData_Handler extends _ATBasicUSRpc_Handler<GuildRequestLoadRankData>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildRequestLoadRankData _rpc)
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

			//设置需要加载
			guildInfo.setNeedLoadRankData();

			_rpc.commit();
    	});
	}
}
