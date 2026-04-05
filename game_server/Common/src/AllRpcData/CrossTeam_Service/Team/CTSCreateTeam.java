package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSCreateTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSCreateTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="创建队伍")
public class CTSCreateTeam extends _ARPCBase<CTSCreateTeam_Req, CTSCreateTeam_Return>
{

	@Override
	protected CTSCreateTeam_Req createRequest()
	{
		return new CTSCreateTeam_Req();
	}

	@Override
	protected CTSCreateTeam_Return createResponse()
	{	
		return new CTSCreateTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSCreateTeam.ordinal();
	}
}
