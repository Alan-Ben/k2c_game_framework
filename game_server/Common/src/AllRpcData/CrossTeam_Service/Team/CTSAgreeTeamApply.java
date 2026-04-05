package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSAgreeTeamApply_Req;
import ALLRPC.CrossTeamServer.Team.CTSAgreeTeamApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="同意玩家入队申请")
public class CTSAgreeTeamApply extends _ARPCBase<CTSAgreeTeamApply_Req, CTSAgreeTeamApply_Return>
{

	@Override
	protected CTSAgreeTeamApply_Req createRequest()
	{
		return new CTSAgreeTeamApply_Req();
	}

	@Override
	protected CTSAgreeTeamApply_Return createResponse()
	{	
		return new CTSAgreeTeamApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSAgreeTeamApply.ordinal();
	}
}
