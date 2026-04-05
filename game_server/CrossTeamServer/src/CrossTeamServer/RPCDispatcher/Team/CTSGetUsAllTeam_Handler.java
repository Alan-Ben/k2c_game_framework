package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSGetUsAllTeam;
import Common.ServerObj.ServerObj_ActivityTeamGroupList;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：获取队长是指定US的所有队伍列表
 */
public class CTSGetUsAllTeam_Handler extends RpcRequestHandler<CTSGetUsAllTeam>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSGetUsAllTeam _rpc)
	{
        for(int i = 0; i < _rpc.req().getGroupIdList().size(); i++)
        {
            CrossGroup group = CrossGroupMgr.getInstance().lookup(_rpc.req().getGroupIdList().get(i));
            if(null == group)
                continue;

            ServerObj_ActivityTeamGroupList groupObj = new ServerObj_ActivityTeamGroupList();
            groupObj.setGroupId(group.getGroupId());
            group.getTeamMgr().makeUsLeaderTeamList(_rpc.req().getUsId(), groupObj.getTeamList());

            _rpc.retObj().addGroupTeamList(groupObj);
        }

		_rpc.commit();
	}
}
