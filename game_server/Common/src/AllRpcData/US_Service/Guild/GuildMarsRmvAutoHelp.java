package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMarsRmvAutoHelp_Req;
import ALLRPC.US.Guild.GuildMarsRmvAutoHelp_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会自动互助移除")
public class GuildMarsRmvAutoHelp extends _ARPCBase<GuildMarsRmvAutoHelp_Req, GuildMarsRmvAutoHelp_Return>
{

	@Override
	protected GuildMarsRmvAutoHelp_Req createRequest()
	{
		return new GuildMarsRmvAutoHelp_Req();
	}

	@Override
	protected GuildMarsRmvAutoHelp_Return createResponse()
	{	
		return new GuildMarsRmvAutoHelp_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMarsRmvAutoHelp.ordinal();
	}
}
