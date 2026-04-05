package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildAddGuildBox;
import NPCommon.ErrMain.GuildErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildBox.GuildBoxTypeMgr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildAddGuildBox_Handler extends _ATBasicUSRpc_Handler<GuildAddGuildBox>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildAddGuildBox _rpc)
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

			GuildBoxTypeMgr typeMgr = guildInfo.getGuildBoxMgr().getTypeMgr(_rpc.req().getBoxType());
			if(null == typeMgr)
			{
				USLog.error(_usServer, "player:{} box:{} type:{} guild box gain fail, not find mgr.", _rpc.req().getCid(), _rpc.req().getBoxId(), _rpc.req().getBoxType());
				return;
			}

			typeMgr.addBox(_rpc.req().getCid(), _rpc.req().getBoxId(), _rpc.req().getCount(), context);

			_rpc.commit();
    	});
	}
}
