package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSGetGroupTeamList;
import Common.CrossTeamObj.CrossTeam_BaseInfo;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;

/*******
 * 处理逻辑：获取指定分组的分页队伍列表
 */
public class CTSGetGroupTeamList_Handler extends RpcRequestHandler<CTSGetGroupTeamList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSGetGroupTeamList _rpc)
	{
		CrossGroup group = CrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
		if (null == group)
		{
			_rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
			return;
		}

		// 获取总数量
		int totalCount = group.getTeamMgr().getTotalCount();
		_rpc.retObj().setTotalCount(totalCount);

		// 填充当前页的队伍基础数据
		ArrayList<CrossTeam_BaseInfo> teamBaseList = new ArrayList<>();
		group.getTeamMgr().makeTeamListByPage(_rpc.req().getPage(), teamBaseList);
		for (CrossTeam_BaseInfo baseInfo : teamBaseList)
			_rpc.retObj().addTeamBaseList(baseInfo);

		_rpc.commit();
	}
}
