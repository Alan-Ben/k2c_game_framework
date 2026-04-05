package USServer.RPCDispatcher.Team;

import AllRpcData.US_Service.Team.UsNotifySyncGroupTeam;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/**
 * 处理逻辑：接收 CTS 通知，向 GLS 同步 Group 队伍成员数据
 */
public class UsNotifySyncGroupTeam_Handler extends _ATBasicUSRpc_Handler<UsNotifySyncGroupTeam> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsNotifySyncGroupTeam _rpc)
    {
        _AActivityBase activity = _usServer.getCommActivityMgr().lookupActivityByGroupId(_rpc.req().getGroupId());
        if (null == activity)
        {
            _rpc.commitFail(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 通知 GLS 同步指定 group 的队伍成员数据
        activity.getGameLogicDealer().syncGroupTeamData(_rpc.req().getGroupId(), _rpc.req().getTeamId());
        _rpc.commit();
    }
}
