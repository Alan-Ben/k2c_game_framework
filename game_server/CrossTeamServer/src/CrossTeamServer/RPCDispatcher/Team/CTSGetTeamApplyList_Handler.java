package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSGetTeamApplyList;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：获取玩家的队伍信息
 */
public class CTSGetTeamApplyList_Handler extends RpcRequestHandler<CTSGetTeamApplyList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSGetTeamApplyList _rpc)
	{
        CrossGroup group = CrossGroupMgr.getInstance().lookupByTeamId(_rpc.req().getTeamId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }

        CrossTeamInfo team = group.getTeamMgr().lookup(_rpc.req().getTeamId());
        if(null == team)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_NOT_FOUND.getCode());
            return;
        }

        team.getApplyMgr().makeApplyList(_rpc.retObj().getApplyList());

		_rpc.commit();
	}
}
