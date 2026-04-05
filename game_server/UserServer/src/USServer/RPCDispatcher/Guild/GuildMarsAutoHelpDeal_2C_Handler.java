package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildMarsAutoHelpDeal_2C;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_GuildMarsHelpAutoDeal;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildMarsAutoHelpDeal_2C_Handler extends _ATBasicUSRpc_Handler<GuildMarsAutoHelpDeal_2C> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildMarsAutoHelpDeal_2C _rpc)
    {
        // 数据计入离线数据，用于玩家获取互助奖励
        if (_rpc.req().getDealCount() > 0)
        {
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_MARS_HELP_AUTO);

            // 处理玩家数据
            Offline_GuildMarsHelpAutoDeal obj = new Offline_GuildMarsHelpAutoDeal();
            obj.setDealedCount(_rpc.req().getDealCount());
            OfflineRewardFunc.addPlayerOfflineReward(_usServer, _rpc.req().getCid(), EOfflineRewardEnum.GUILD_MARS_HELP_AUTO_DEAL, obj, null, null, context);
        }

        _rpc.commit();
    }
}