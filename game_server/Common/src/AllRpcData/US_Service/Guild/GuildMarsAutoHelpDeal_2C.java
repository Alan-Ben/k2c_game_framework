package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMarsAutoHelpDeal_2C_Req;
import ALLRPC.US.Guild.GuildMarsAutoHelpDeal_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会自动互助处理")
public class GuildMarsAutoHelpDeal_2C extends _ARPCBase<GuildMarsAutoHelpDeal_2C_Req, GuildMarsAutoHelpDeal_2C_Return>
{

	@Override
	protected GuildMarsAutoHelpDeal_2C_Req createRequest()
	{
		return new GuildMarsAutoHelpDeal_2C_Req();
	}

	@Override
	protected GuildMarsAutoHelpDeal_2C_Return createResponse()
	{	
		return new GuildMarsAutoHelpDeal_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMarsEnsureAutoDeal_2C.ordinal();
	}
}
