package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildRmvMarsHelp_Req;
import ALLRPC.US.Guild.GuildRmvMarsHelp_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="移除火星互助数据")
public class GuildRmvMarsHelp extends _ARPCBase<GuildRmvMarsHelp_Req, GuildRmvMarsHelp_Return>
{

	@Override
	protected GuildRmvMarsHelp_Req createRequest()
	{
		return new GuildRmvMarsHelp_Req();
	}

	@Override
	protected GuildRmvMarsHelp_Return createResponse()
	{	
		return new GuildRmvMarsHelp_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildRmvMarsHelp.ordinal();
	}
}
