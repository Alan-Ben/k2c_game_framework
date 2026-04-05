package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取玩家的队伍信息")
public class CTSGetTeam extends _ARPCBase<CTSGetTeam_Req, CTSGetTeam_Return>
{

	@Override
	protected CTSGetTeam_Req createRequest()
	{
		return new CTSGetTeam_Req();
	}

	@Override
	protected CTSGetTeam_Return createResponse()
	{	
		return new CTSGetTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetTeam.ordinal();
	}
}
