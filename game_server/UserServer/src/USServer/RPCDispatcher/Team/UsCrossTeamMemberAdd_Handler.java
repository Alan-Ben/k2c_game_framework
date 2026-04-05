package USServer.RPCDispatcher.Team;

import AllRpcData.US_Service.Team.UsCrossTeamMemberAdd;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Activities.FirstTeamActivity.FirstTeamActivity;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 增加队伍成员
 */
public class UsCrossTeamMemberAdd_Handler extends _ATBasicUSRpc_Handler<UsCrossTeamMemberAdd> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsCrossTeamMemberAdd _rpc)
	{
        _AActivityBase activity = _usServer.getCommActivityMgr().lookupActivityByGroupId(_rpc.req().getGroupId());
        if(!(activity instanceof FirstTeamActivity))
        {
            _rpc.commitFail(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

		_rpc.commit();
	}
}
