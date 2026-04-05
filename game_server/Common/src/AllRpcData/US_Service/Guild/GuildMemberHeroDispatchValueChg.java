package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMemberHeroDispatchValueChg_Req;
import ALLRPC.US.Guild.GuildMemberHeroDispatchValueChg_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 公会玩家派遣大臣属性变化")
public class GuildMemberHeroDispatchValueChg extends _ARPCBase<GuildMemberHeroDispatchValueChg_Req, GuildMemberHeroDispatchValueChg_Return>
{

	@Override
	protected GuildMemberHeroDispatchValueChg_Req createRequest()
	{
		return new GuildMemberHeroDispatchValueChg_Req();
	}

	@Override
	protected GuildMemberHeroDispatchValueChg_Return createResponse()
	{	
		return new GuildMemberHeroDispatchValueChg_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMemberHeroDispatchValueChg.ordinal();
	}
}
