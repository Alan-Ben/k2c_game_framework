package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetUsAllTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetUsAllTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取队长是指定US的所有队伍列表")
public class CTSGetUsAllTeam extends _ARPCBase<CTSGetUsAllTeam_Req, CTSGetUsAllTeam_Return>
{

	@Override
	protected CTSGetUsAllTeam_Req createRequest()
	{
		return new CTSGetUsAllTeam_Req();
	}

	@Override
	protected CTSGetUsAllTeam_Return createResponse()
	{	
		return new CTSGetUsAllTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetUsAllTeam.ordinal();
	}
}
