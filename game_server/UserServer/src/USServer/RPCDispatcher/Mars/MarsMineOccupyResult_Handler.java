package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineOccupyResult;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑：火星矿占领结果数据
 * 此处理是进攻方处理之后发送的 RPC 消息处理函数
 */
public class MarsMineOccupyResult_Handler extends _ATBasicUSRpc_Handler<MarsMineOccupyResult> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, MarsMineOccupyResult _rpc)
    {
        OfflineRewardFunc.addPlayerOfflineReward(
                _usServer,
                _rpc.req().getResult().getCid(),
                EOfflineRewardEnum.MARS_TEAM_OCCUPY_RESULT,
                _rpc.req().getResult(),
                null,
                null,
                NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));

        _rpc.commit();
    }
}