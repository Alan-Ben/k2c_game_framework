package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildAddGuildPoint;
import NPCommon.ErrMain.GuildErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildAddGuildPoint_Handler extends _ATBasicUSRpc_Handler<GuildAddGuildPoint>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildAddGuildPoint _rpc)
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

			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ENPGameEvent_FromInt(_rpc.req().getContextType()));
			context.setGuid(_rpc.req().getContextGuid());

			guildInfo.incrActivePoint(_rpc.req().getAddPoint(), context);

			_rpc.commit();
    	});
	}
}
