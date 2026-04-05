package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildGuildRequest_Req;
import ALLRPC.US.Guild.GuildGuildRequest_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="移除公会记录的申请信息")
public class GuildRmvGuildRequest extends _ARPCBase<GuildGuildRequest_Req, GuildGuildRequest_Return>
{

	@Override
	protected GuildGuildRequest_Req createRequest()
	{
		return new GuildGuildRequest_Req();
	}

	@Override
	protected GuildGuildRequest_Return createResponse()
	{	
		return new GuildGuildRequest_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildRmvGuildRequest.ordinal();
	}
}
