package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildAddMarsBattleReport_Req;
import ALLRPC.US.Guild.GuildAddMarsBattleReport_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" add mars mine guild battle report")
public class GuildAddMarsBattleReport extends _ARPCBase<GuildAddMarsBattleReport_Req, GuildAddMarsBattleReport_Return>
{

	@Override
	protected GuildAddMarsBattleReport_Req createRequest()
	{
		return new GuildAddMarsBattleReport_Req();
	}

	@Override
	protected GuildAddMarsBattleReport_Return createResponse()
	{	
		return new GuildAddMarsBattleReport_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildAddMarsBattleReport.ordinal();
	}
}
