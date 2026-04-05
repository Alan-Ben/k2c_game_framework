package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildGetDispatchHeroInfo;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.HeroErr;
import NPUSServer.Guild.Dispatch.GuildHeroDispatchInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildGetDispatchHeroInfo_Handler extends _ATBasicUSRpc_Handler<GuildGetDispatchHeroInfo>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildGetDispatchHeroInfo _rpc)
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

			//查询大臣派遣记录
			GuildHeroDispatchInfo heroDispatchInfo = guildInfo.getHeroDispatchMgr().lookupByCid(_rpc.req().getCid());
			if(heroDispatchInfo.getHeroId() != _rpc.req().getHeroId())
			{
				_rpc.commitFail(HeroErr.HERO_NOT_FOUND.getCode());
				return ;
			}

			//返回信息
			_rpc.retObj().setHeroId(_rpc.req().getHeroId());
			_rpc.retObj().setPower(heroDispatchInfo.getPower());

			_rpc.commit();
    	});
	}
}
