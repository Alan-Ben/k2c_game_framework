package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSQuitTeamGroup_Req;
import ALLRPC.CrossTeamServer.Team.CTSQuitTeamGroup_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="退出跨服队伍分组")
public class CTSQuitTeamGroup extends _ARPCBase<CTSQuitTeamGroup_Req, CTSQuitTeamGroup_Return>
{

	@Override
	protected CTSQuitTeamGroup_Req createRequest()
	{
		return new CTSQuitTeamGroup_Req();
	}

	@Override
	protected CTSQuitTeamGroup_Return createResponse()
	{	
		return new CTSQuitTeamGroup_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSQuitTeamGroup.ordinal();
	}
}
