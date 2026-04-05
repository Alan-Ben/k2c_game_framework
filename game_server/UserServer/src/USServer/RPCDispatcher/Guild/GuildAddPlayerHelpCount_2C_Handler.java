package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildAddPlayerHelpCount_2C;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家被帮助信息
 */
public class GuildAddPlayerHelpCount_2C_Handler extends _ATBasicUSRpc_Handler<GuildAddPlayerHelpCount_2C> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildAddPlayerHelpCount_2C _rpc)
    {
        OfflineRewardFunc.addPlayerOfflineReward(
                _usServer,
                _rpc.req().getCid(),
                EOfflineRewardEnum.GUILD_MARS_HELP_SUC,
                _rpc.req().get_buffer_HelpSucData(),
                NPPlayerContext.createNew(ENPGameEvent.GUILD_MARS_HELP_BE_DEALED));

        _rpc.commit();
    }
}
