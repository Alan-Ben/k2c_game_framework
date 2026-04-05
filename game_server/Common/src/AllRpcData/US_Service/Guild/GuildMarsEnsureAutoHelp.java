package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMarsEnsureAutoHelp_Req;
import ALLRPC.US.Guild.GuildMarsEnsureAutoHelp_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会自动互助添加")
public class GuildMarsEnsureAutoHelp extends _ARPCBase<GuildMarsEnsureAutoHelp_Req, GuildMarsEnsureAutoHelp_Return>
{

	@Override
	protected GuildMarsEnsureAutoHelp_Req createRequest()
	{
		return new GuildMarsEnsureAutoHelp_Req();
	}

	@Override
	protected GuildMarsEnsureAutoHelp_Return createResponse()
	{	
		return new GuildMarsEnsureAutoHelp_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMarsEnsureAutoHelp.ordinal();
	}
}
