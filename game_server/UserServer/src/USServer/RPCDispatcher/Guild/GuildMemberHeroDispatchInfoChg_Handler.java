package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMemberHeroDispatchInfoChg;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildMemberHeroDispatchInfoChg_Handler extends _ATBasicUSRpc_Handler<GuildMemberHeroDispatchInfoChg>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMemberHeroDispatchInfoChg _rpc)
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

			//处理玩家派遣数据变化
			guildInfo.getHeroDispatchMgr().onDispatchHeroInfoChg(_rpc.req().getCid(), _rpc.req().getHeroId(), _rpc.req().getLevel(), _rpc.req().getPower(), _rpc.req().getSkinId());
    		
    		_rpc.commit();
    	});
	}
}
