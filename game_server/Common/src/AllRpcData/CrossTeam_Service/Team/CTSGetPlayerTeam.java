package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetPlayerTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetPlayerTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取玩家的队伍信息")
public class CTSGetPlayerTeam extends _ARPCBase<CTSGetPlayerTeam_Req, CTSGetPlayerTeam_Return>
{

	@Override
	protected CTSGetPlayerTeam_Req createRequest()
	{
		return new CTSGetPlayerTeam_Req();
	}

	@Override
	protected CTSGetPlayerTeam_Return createResponse()
	{	
		return new CTSGetPlayerTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetPlayerTeam.ordinal();
	}
}
