package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildAddPlayerHelpCount_2C_Req;
import ALLRPC.US.Guild.GuildAddPlayerHelpCount_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="增加玩家被帮助信息")
public class GuildAddPlayerHelpCount_2C extends _ARPCBase<GuildAddPlayerHelpCount_2C_Req, GuildAddPlayerHelpCount_2C_Return>
{

	@Override
	protected GuildAddPlayerHelpCount_2C_Req createRequest()
	{
		return new GuildAddPlayerHelpCount_2C_Req();
	}

	@Override
	protected GuildAddPlayerHelpCount_2C_Return createResponse()
	{	
		return new GuildAddPlayerHelpCount_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildAddPlayerHelpCount_2C.ordinal();
	}
}
