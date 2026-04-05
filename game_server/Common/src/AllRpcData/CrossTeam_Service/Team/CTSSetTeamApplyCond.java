package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSSetTeamApplyCond_Req;
import ALLRPC.CrossTeamServer.Team.CTSSetTeamApplyCond_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="设置队伍申请条件")
public class CTSSetTeamApplyCond extends _ARPCBase<CTSSetTeamApplyCond_Req, CTSSetTeamApplyCond_Return>
{

	@Override
	protected CTSSetTeamApplyCond_Req createRequest()
	{
		return new CTSSetTeamApplyCond_Req();
	}

	@Override
	protected CTSSetTeamApplyCond_Return createResponse()
	{	
		return new CTSSetTeamApplyCond_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSSetTeamApplyCond.ordinal();
	}
}
