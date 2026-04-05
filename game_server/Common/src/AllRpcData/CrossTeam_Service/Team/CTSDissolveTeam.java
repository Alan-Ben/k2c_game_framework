package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSDissolveTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSDissolveTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="解散跨服队伍")
public class CTSDissolveTeam extends _ARPCBase<CTSDissolveTeam_Req, CTSDissolveTeam_Return>
{

	@Override
	protected CTSDissolveTeam_Req createRequest()
	{
		return new CTSDissolveTeam_Req();
	}

	@Override
	protected CTSDissolveTeam_Return createResponse()
	{	
		return new CTSDissolveTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSDissolveTeam.ordinal();
	}
}
