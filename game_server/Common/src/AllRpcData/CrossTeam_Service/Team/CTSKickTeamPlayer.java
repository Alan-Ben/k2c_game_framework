package AllRpcData.CrossTeam_Service.Team;

import ALLRPC.CrossTeamServer.Team.CTSKickTeamPlayer_Req;
import ALLRPC.CrossTeamServer.Team.CTSKickTeamPlayer_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="踢出队伍成员")
public class CTSKickTeamPlayer extends _ARPCBase<CTSKickTeamPlayer_Req, CTSKickTeamPlayer_Return>
{

	@Override
	protected CTSKickTeamPlayer_Req createRequest()
	{
		return new CTSKickTeamPlayer_Req();
	}

	@Override
	protected CTSKickTeamPlayer_Return createResponse()
	{	
		return new CTSKickTeamPlayer_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CTSKickTeamPlayer.ordinal();
	}
}
