package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSRefuseTeamApply_Req;
import ALLRPC.CrossTeamServer.Team.CTSRefuseTeamApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="拒绝玩家入队申请")
public class CTSRefuseTeamApply extends _ARPCBase<CTSRefuseTeamApply_Req, CTSRefuseTeamApply_Return>
{

	@Override
	protected CTSRefuseTeamApply_Req createRequest()
	{
		return new CTSRefuseTeamApply_Req();
	}

	@Override
	protected CTSRefuseTeamApply_Return createResponse()
	{	
		return new CTSRefuseTeamApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSRefuseTeamApply.ordinal();
	}
}
