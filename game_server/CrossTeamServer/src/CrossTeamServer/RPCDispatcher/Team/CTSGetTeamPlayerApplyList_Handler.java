package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSGetTeamPlayerApplyList;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：获取玩家发起请求数据列表
 */
public class CTSGetTeamPlayerApplyList_Handler extends RpcRequestHandler<CTSGetTeamPlayerApplyList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSGetTeamPlayerApplyList _rpc)
	{
        CrossGroup group = CrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }

        group.getPlayerApplyMgr().makePlayerApplyList(_rpc.req().getCid(), _rpc.retObj().getApplyList());

		_rpc.commit();
	}
}
