package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMemberHeroDispatchInfoChg_Req;
import ALLRPC.US.Guild.GuildMemberHeroDispatchInfoChg_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会玩家派遣大臣信息变化")
public class GuildMemberHeroDispatchInfoChg extends _ARPCBase<GuildMemberHeroDispatchInfoChg_Req, GuildMemberHeroDispatchInfoChg_Return>
{

	@Override
	protected GuildMemberHeroDispatchInfoChg_Req createRequest()
	{
		return new GuildMemberHeroDispatchInfoChg_Req();
	}

	@Override
	protected GuildMemberHeroDispatchInfoChg_Return createResponse()
	{	
		return new GuildMemberHeroDispatchInfoChg_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMemberHeroDispatchInfoChg.ordinal();
	}
}
