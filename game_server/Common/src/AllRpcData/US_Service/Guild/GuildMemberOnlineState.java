package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMemberOnlineState_Req;
import ALLRPC.US.Guild.GuildMemberOnlineState_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会玩家上下线状态同步")
public class GuildMemberOnlineState extends _ARPCBase<GuildMemberOnlineState_Req, GuildMemberOnlineState_Return>
{

	@Override
	protected GuildMemberOnlineState_Req createRequest()
	{
		return new GuildMemberOnlineState_Req();
	}

	@Override
	protected GuildMemberOnlineState_Return createResponse()
	{	
		return new GuildMemberOnlineState_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMemberOnlineState.ordinal();
	}
}
