package AllRpcData.US_Service.Team;

import ALLRPC.US.Team.UsCrossTeamMemberAdd_Req;
import ALLRPC.US.Team.UsCrossTeamMemberAdd_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 增加队伍成员")
public class UsCrossTeamMemberAdd extends _ARPCBase<UsCrossTeamMemberAdd_Req, UsCrossTeamMemberAdd_Return>
{

	@Override
	protected UsCrossTeamMemberAdd_Req createRequest()
	{
		return new UsCrossTeamMemberAdd_Req();
	}

	@Override
	protected UsCrossTeamMemberAdd_Return createResponse()
	{	
		return new UsCrossTeamMemberAdd_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsCrossTeamMemberAdd.ordinal();
	}
}
