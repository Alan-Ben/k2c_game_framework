package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSGetPlayerTeamRoom_Req;
import ALLRPC.CrossTeamServer.Team.CTSGetPlayerTeamRoom_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取玩家的队伍聊天房间ID")
public class CTSGetPlayerTeamRoom extends _ARPCBase<CTSGetPlayerTeamRoom_Req, CTSGetPlayerTeamRoom_Return>
{

	@Override
	protected CTSGetPlayerTeamRoom_Req createRequest()
	{
		return new CTSGetPlayerTeamRoom_Req();
	}

	@Override
	protected CTSGetPlayerTeamRoom_Return createResponse()
	{	
		return new CTSGetPlayerTeamRoom_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSGetPlayerTeamRoom.ordinal();
	}
}
