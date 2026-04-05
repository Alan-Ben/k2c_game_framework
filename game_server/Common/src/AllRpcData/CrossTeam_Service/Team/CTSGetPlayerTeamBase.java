package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetPlayerTeamBase_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetPlayerTeamBase_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取玩家的队伍基础信息")
public class CTSGetPlayerTeamBase extends _ARPCBase<CTSGetPlayerTeamBase_Req, CTSGetPlayerTeamBase_Return>
{

	@Override
	protected CTSGetPlayerTeamBase_Req createRequest()
	{
		return new CTSGetPlayerTeamBase_Req();
	}

	@Override
	protected CTSGetPlayerTeamBase_Return createResponse()
	{	
		return new CTSGetPlayerTeamBase_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetPlayerTeamBase.ordinal();
	}
}
