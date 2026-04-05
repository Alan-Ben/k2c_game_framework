package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMemberOnlineState;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildMemberOnlineState_Handler extends _ATBasicUSRpc_Handler<GuildMemberOnlineState>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMemberOnlineState _rpc)
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

			//同步玩家状态
			if(_rpc.req().getIsOnLine()) {
				guildInfo.getMemberMgr().onMemberOnline(_rpc.req().getCid(), _rpc.req().getTimeMS());
			}
			else
			{
				guildInfo.getMemberMgr().onMemberOffline(_rpc.req().getCid(), _rpc.req().getTimeMS());
			}
    		
    		_rpc.commit();
    	});
	}
}
