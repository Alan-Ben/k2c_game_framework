package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSTeamBroadcastMsg_Req;
import ALLRPC.CrossTeamServer.Team.CTSTeamBroadcastMsg_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="队伍成员广播消息")
public class CTSTeamBroadcastMsg extends _ARPCBase<CTSTeamBroadcastMsg_Req, CTSTeamBroadcastMsg_Return>
{

	@Override
	protected CTSTeamBroadcastMsg_Req createRequest()
	{
		return new CTSTeamBroadcastMsg_Req();
	}

	@Override
	protected CTSTeamBroadcastMsg_Return createResponse()
	{	
		return new CTSTeamBroadcastMsg_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSTeamBroadcastMsg.ordinal();
	}
}
