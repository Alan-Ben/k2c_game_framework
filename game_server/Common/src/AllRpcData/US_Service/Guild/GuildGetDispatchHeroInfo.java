package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildGetDispatchHeroInfo_Req;
import ALLRPC.US.Guild.GuildGetDispatchHeroInfo_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取玩家派遣英雄信息")
public class GuildGetDispatchHeroInfo extends _ARPCBase<GuildGetDispatchHeroInfo_Req, GuildGetDispatchHeroInfo_Return>
{

	@Override
	protected GuildGetDispatchHeroInfo_Req createRequest()
	{
		return new GuildGetDispatchHeroInfo_Req();
	}

	@Override
	protected GuildGetDispatchHeroInfo_Return createResponse()
	{	
		return new GuildGetDispatchHeroInfo_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildGetDispatchHeroInfo.ordinal();
	}
}
