package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildPlayerRequestDungeonLvl;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildDungeon.GuildDungeonSetInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildPlayerRequestDungeonLvl_Handler extends _ATBasicUSRpc_Handler<GuildPlayerRequestDungeonLvl>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildPlayerRequestDungeonLvl _rpc)
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

			//检查副本配置数据
			GuildDungeonSetInfo setInfo = guildInfo.getDungeonMgr().getSetMgr().lookup(_rpc.req().getDungeonId());
			if(null == setInfo)
			{
				_rpc.commitFail(GuildErr.GUILD_DUNGEON_NOT_FOUND.getCode());
				return ;
			}
			if(!setInfo.isUnlock())
			{
				_rpc.commitFail(GuildErr.GUILD_DUNGEON_NOT_UNLOCK.getCode());
				return ;
			}

			//检查是否还有正在运行的副本
			if(guildInfo.getDungeonMgr().getInstanceMgr().hasDungeon(_rpc.req().getDungeonId()))
			{
				_rpc.commitFail(GuildErr.GUILD_DUNGEON_STARTED.getCode());
				return ;
			}

			_rpc.retObj().setLvl(setInfo.getLvl());
    		
    		_rpc.commit();
    	});
	}
}
