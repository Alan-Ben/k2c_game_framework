package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetGroupTeamList_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetGroupTeamList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取指定分组的分页队伍列表")
public class CTSGetGroupTeamList extends _ARPCBase<CTSGetGroupTeamList_Req, CTSGetGroupTeamList_Return>
{

	@Override
	protected CTSGetGroupTeamList_Req createRequest()
	{
		return new CTSGetGroupTeamList_Req();
	}

	@Override
	protected CTSGetGroupTeamList_Return createResponse()
	{	
		return new CTSGetGroupTeamList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetGroupTeamList.ordinal();
	}
}
