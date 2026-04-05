package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSSetTeamSetting_Req;
import ALLRPC.CrossTeamServer.Team.CTSSetTeamSetting_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="修改队伍设置")
public class CTSSetTeamSetting extends _ARPCBase<CTSSetTeamSetting_Req, CTSSetTeamSetting_Return>
{

	@Override
	protected CTSSetTeamSetting_Req createRequest()
	{
		return new CTSSetTeamSetting_Req();
	}

	@Override
	protected CTSSetTeamSetting_Return createResponse()
	{	
		return new CTSSetTeamSetting_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSSetTeamSetting.ordinal();
	}
}
