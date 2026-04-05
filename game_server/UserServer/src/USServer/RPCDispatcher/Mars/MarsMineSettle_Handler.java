package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineSettle;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑：火星矿结算数据
 * 矿产结算后会发送该 rpc 到 US，由 US 处理相关玩家队伍状态变更
 */
public class MarsMineSettle_Handler extends _ATBasicUSRpc_Handler<MarsMineSettle> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, MarsMineSettle _rpc)
    {
        // 处理玩家数据
        OfflineRewardFunc.addPlayerOfflineReward(
                _usServer,
                _rpc.req().getInfo().getCid(),
                EOfflineRewardEnum.MARS_MINE_TEAM_SETTLE,
                _rpc.req().getInfo(),
                null,
                null,
                NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_COLLECT_END));

        _rpc.commit();
    }
}