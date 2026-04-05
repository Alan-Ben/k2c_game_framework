package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSQuitTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSQuitTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="退出队伍")
public class CTSQuitTeam extends _ARPCBase<CTSQuitTeam_Req, CTSQuitTeam_Return>
{

	@Override
	protected CTSQuitTeam_Req createRequest()
	{
		return new CTSQuitTeam_Req();
	}

	@Override
	protected CTSQuitTeam_Return createResponse()
	{	
		return new CTSQuitTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSQuitTeam.ordinal();
	}
}
