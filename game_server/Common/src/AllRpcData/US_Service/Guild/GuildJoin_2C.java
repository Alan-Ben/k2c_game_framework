package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildJoin_2C_Req;
import ALLRPC.US.Guild.GuildJoin_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会加入的User部分处理协议")
public class GuildJoin_2C extends _ARPCBase<GuildJoin_2C_Req, GuildJoin_2C_Return>
{
	@Override
	protected GuildJoin_2C_Req createRequest()
	{
		return new GuildJoin_2C_Req();
	}

	@Override
	protected GuildJoin_2C_Return createResponse()
	{	
		return new GuildJoin_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildJoin_2C.ordinal();
	}
}
