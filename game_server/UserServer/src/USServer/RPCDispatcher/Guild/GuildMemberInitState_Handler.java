package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMemberInitState;
import CommonEnum.ESpecAttrType;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildMemberInitState_Handler extends _ATBasicUSRpc_Handler<GuildMemberInitState>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMemberInitState _rpc)
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

			//返回公会信息
			_rpc.retObj().setGuildName(guildInfo.getName());
			_rpc.retObj().setGuildSimpleName(guildInfo.getSimpleName());
			_rpc.retObj().setGuildLvl(guildInfo.getLevel());
			for(int i = 0; i < ESpecAttrType.values().length; i++) {
				_rpc.retObj().getBuildingAddPerArr().add(guildInfo.getHeroDispatchMgr().getAddValue(ESpecAttrType.ESpecAttrType_FromInt(i)));
			}
    		
    		_rpc.commit();
    	});
	}
}
