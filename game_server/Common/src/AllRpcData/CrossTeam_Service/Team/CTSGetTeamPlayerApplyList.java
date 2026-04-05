package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetTeamPlayerApplyList_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetTeamPlayerApplyList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取玩家发起请求数据列表")
public class CTSGetTeamPlayerApplyList extends _ARPCBase<CTSGetTeamPlayerApplyList_Req, CTSGetTeamPlayerApplyList_Return>
{

	@Override
	protected CTSGetTeamPlayerApplyList_Req createRequest()
	{
		return new CTSGetTeamPlayerApplyList_Req();
	}

	@Override
	protected CTSGetTeamPlayerApplyList_Return createResponse()
	{	
		return new CTSGetTeamPlayerApplyList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetTeamPlayerApplyList.ordinal();
	}
}
