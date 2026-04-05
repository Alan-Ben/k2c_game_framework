package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMemberRmv_2C_Req;
import ALLRPC.US.Guild.GuildMemberRmv_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="玩家退出公会的处理")
public class GuildMemberRmv_2C extends _ARPCBase<GuildMemberRmv_2C_Req, GuildMemberRmv_2C_Return>
{
	@Override
	protected GuildMemberRmv_2C_Req createRequest()
	{
		return new GuildMemberRmv_2C_Req();
	}

	@Override
	protected GuildMemberRmv_2C_Return createResponse()
	{	
		return new GuildMemberRmv_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMemberRmv_2C.ordinal();
	}
}
