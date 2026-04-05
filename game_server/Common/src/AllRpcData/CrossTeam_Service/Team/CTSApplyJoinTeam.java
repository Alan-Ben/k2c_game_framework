package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSApplyJoinTeam_Req;
import ALLRPC.CrossTeamServer.Team.CTSApplyJoinTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="申请加入队伍")
public class CTSApplyJoinTeam extends _ARPCBase<CTSApplyJoinTeam_Req, CTSApplyJoinTeam_Return>
{

	@Override
	protected CTSApplyJoinTeam_Req createRequest()
	{
		return new CTSApplyJoinTeam_Req();
	}

	@Override
	protected CTSApplyJoinTeam_Return createResponse()
	{	
		return new CTSApplyJoinTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSApplyJoinTeam.ordinal();
	}
}
