package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildAddGuildBox_Req;
import ALLRPC.US.Guild.GuildAddGuildBox_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="增加公会宝箱")
public class GuildAddGuildBox extends _ARPCBase<GuildAddGuildBox_Req, GuildAddGuildBox_Return>
{

	@Override
	protected GuildAddGuildBox_Req createRequest()
	{
		return new GuildAddGuildBox_Req();
	}

	@Override
	protected GuildAddGuildBox_Return createResponse()
	{	
		return new GuildAddGuildBox_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildAddGuildBox.ordinal();
	}
}
