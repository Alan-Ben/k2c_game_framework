package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsRallyJoinFailBack;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑：集结加入失败后遣返队伍
 */
public class MarsRallyJoinFailBack_Handler extends _ATBasicUSRpc_Handler<MarsRallyJoinFailBack> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, MarsRallyJoinFailBack _rpc)
    {
        // 通过 cid 定位玩家，在线与离线统一写入 OfflineReward 链路
        OfflineRewardFunc.addPlayerOfflineReward(
                _usServer,
                _rpc.req().getCid(),
                EOfflineRewardEnum.MARS_RALLY_JOIN_FAIL_BACK,
                _rpc.req(),
                null,
                null,
                NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_STATUS_CHG));

        // RPC 层只负责投递事件，具体状态机推进由 OfflineDealer 统一处理
        _rpc.commit();
    }
}