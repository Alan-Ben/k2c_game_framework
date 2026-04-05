package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMemberInitState_Req;
import ALLRPC.US.Guild.GuildMemberInitState_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会玩家初始化")
public class GuildMemberInitState extends _ARPCBase<GuildMemberInitState_Req, GuildMemberInitState_Return>
{

	@Override
	protected GuildMemberInitState_Req createRequest()
	{
		return new GuildMemberInitState_Req();
	}

	@Override
	protected GuildMemberInitState_Return createResponse()
	{	
		return new GuildMemberInitState_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMemberInitState.ordinal();
	}
}
