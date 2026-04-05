package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSJoinTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSJoinTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="加入队伍")
public class CTSJoinTeam extends _ARPCBase<CTSJoinTeam_Req, CTSJoinTeam_Return>
{

	@Override
	protected CTSJoinTeam_Req createRequest()
	{
		return new CTSJoinTeam_Req();
	}

	@Override
	protected CTSJoinTeam_Return createResponse()
	{	
		return new CTSJoinTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSJoinTeam.ordinal();
	}
}
