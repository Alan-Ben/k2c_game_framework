package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildRequestLoadRankData_Req;
import ALLRPC.US.Guild.GuildRequestLoadRankData_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会需要重新加载战力数据")
public class GuildRequestLoadRankData extends _ARPCBase<GuildRequestLoadRankData_Req, GuildRequestLoadRankData_Return>
{

	@Override
	protected GuildRequestLoadRankData_Req createRequest()
	{
		return new GuildRequestLoadRankData_Req();
	}

	@Override
	protected GuildRequestLoadRankData_Return createResponse()
	{	
		return new GuildRequestLoadRankData_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildRequestLoadRankData.ordinal();
	}
}
