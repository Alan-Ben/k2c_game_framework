package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSRegUsGroup_Req;
import ALLRPC.CrossTeamServer.Team.CTSRegUsGroup_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="注册US到跨服队伍分组")
public class CTSRegUsGroup extends _ARPCBase<CTSRegUsGroup_Req, CTSRegUsGroup_Return>
{

	@Override
	protected CTSRegUsGroup_Req createRequest()
	{
		return new CTSRegUsGroup_Req();
	}

	@Override
	protected CTSRegUsGroup_Return createResponse()
	{	
		return new CTSRegUsGroup_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSRegUsGroup.ordinal();
	}
}
