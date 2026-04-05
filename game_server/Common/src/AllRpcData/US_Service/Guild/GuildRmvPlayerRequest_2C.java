package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildPlayerRequest_2C_Req;
import ALLRPC.US.Guild.GuildPlayerRequest_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="移除玩家申请信息")
public class GuildRmvPlayerRequest_2C extends _ARPCBase<GuildPlayerRequest_2C_Req, GuildPlayerRequest_2C_Return>
{

	@Override
	protected GuildPlayerRequest_2C_Req createRequest()
	{
		return new GuildPlayerRequest_2C_Req();
	}

	@Override
	protected GuildPlayerRequest_2C_Return createResponse()
	{	
		return new GuildPlayerRequest_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildRmvPlayerRequest_2C.ordinal();
	}
}
