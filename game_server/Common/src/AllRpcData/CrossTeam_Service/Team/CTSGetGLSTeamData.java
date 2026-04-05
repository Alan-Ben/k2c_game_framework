package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetGLSTeamData_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetGLSTeamData_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取GLS GroupMgr所需的指定队伍数据")
public class CTSGetGLSTeamData extends _ARPCBase<CTSGetGLSTeamData_Req, CTSGetGLSTeamData_Return>
{

	@Override
	protected CTSGetGLSTeamData_Req createRequest()
	{
		return new CTSGetGLSTeamData_Req();
	}

	@Override
	protected CTSGetGLSTeamData_Return createResponse()
	{	
		return new CTSGetGLSTeamData_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetGLSTeamData.ordinal();
	}
}
