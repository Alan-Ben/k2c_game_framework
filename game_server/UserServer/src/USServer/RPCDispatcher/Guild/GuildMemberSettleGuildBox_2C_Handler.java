package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMemberSettleGuildBox_2C;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 发送公会宝箱
 */
public class GuildMemberSettleGuildBox_2C_Handler extends _ATBasicUSRpc_Handler<GuildMemberSettleGuildBox_2C>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMemberSettleGuildBox_2C _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			//统一走offlineReward处理机制
			OfflineRewardFunc.onPlayerSettleGuildBox(_usServer, _rpc.req().getCid(), _rpc.req().getBoxList(), NPPlayerContext.createNew(ENPGameEvent.GUILD_BOX_SETTLE));

			_rpc.commit();
    	});
	}
}
