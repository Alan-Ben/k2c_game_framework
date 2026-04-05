package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsRallyJoinToWait;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑：集结加入后切换等待集结状态
 */
public class MarsRallyJoinToWait_Handler extends _ATBasicUSRpc_Handler<MarsRallyJoinToWait> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, MarsRallyJoinToWait _rpc)
    {
        OfflineRewardFunc.addPlayerOfflineReward(
                _usServer,
                _rpc.req().getCid(),
                EOfflineRewardEnum.MARS_RALLY_JOIN_TO_WAIT,
                _rpc.req(),
                null,
                null,
                NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_STATUS_CHG));

        // RPC 层只负责投递事件，具体状态机推进由 OfflineDealer 统一处理
        _rpc.commit();
    }
}