package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildAddMarsMine_Req;
import ALLRPC.US.Guild.GuildAddMarsMine_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="添加火星矿分享")
public class GuildAddMarsMine extends _ARPCBase<GuildAddMarsMine_Req, GuildAddMarsMine_Return>
{

	@Override
	protected GuildAddMarsMine_Req createRequest()
	{
		return new GuildAddMarsMine_Req();
	}

	@Override
	protected GuildAddMarsMine_Return createResponse()
	{	
		return new GuildAddMarsMine_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildAddMarsMine.ordinal();
	}
}
