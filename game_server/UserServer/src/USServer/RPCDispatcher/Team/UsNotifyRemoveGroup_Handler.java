package USServer.RPCDispatcher.Team;

import AllRpcData.US_Service.Team.UsNotifyRemoveGroup;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/**
 * 处理逻辑：接收 CTS 通知，向 GLS 移除对应的 Group（队伍解散时触发）
 */
public class UsNotifyRemoveGroup_Handler extends _ATBasicUSRpc_Handler<UsNotifyRemoveGroup> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsNotifyRemoveGroup _rpc)
    {
        _AActivityBase activity = _usServer.getCommActivityMgr().lookupActivityByGroupId(_rpc.req().getGroupId());
        if (null == activity)
        {
            _rpc.commitFail(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 通知 GLS 移除指定 group
        activity.getGameLogicDealer().removeGroupByTeam(_rpc.req().getGroupId());
        _rpc.commit();
    }
}
