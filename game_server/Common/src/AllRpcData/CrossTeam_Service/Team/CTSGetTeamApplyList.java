package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetTeamApplyList_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetTeamApplyList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取队伍申请信息")
public class CTSGetTeamApplyList extends _ARPCBase<CTSGetTeamApplyList_Req, CTSGetTeamApplyList_Return>
{

	@Override
	protected CTSGetTeamApplyList_Req createRequest()
	{
		return new CTSGetTeamApplyList_Req();
	}

	@Override
	protected CTSGetTeamApplyList_Return createResponse()
	{	
		return new CTSGetTeamApplyList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetTeamApplyList.ordinal();
	}
}
