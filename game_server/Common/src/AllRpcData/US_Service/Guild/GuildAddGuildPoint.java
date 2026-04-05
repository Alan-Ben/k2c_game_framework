package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildAddGuildPoint_Req;
import ALLRPC.US.Guild.GuildAddGuildPoint_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="增加公会宝箱活跃点")
public class GuildAddGuildPoint extends _ARPCBase<GuildAddGuildPoint_Req, GuildAddGuildPoint_Return>
{

	@Override
	protected GuildAddGuildPoint_Req createRequest()
	{
		return new GuildAddGuildPoint_Req();
	}

	@Override
	protected GuildAddGuildPoint_Return createResponse()
	{	
		return new GuildAddGuildPoint_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildAddGuildPoint.ordinal();
	}
}
