package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildPlayerRequestDungeonLvl_Req;
import ALLRPC.US.Guild.GuildPlayerRequestDungeonLvl_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取联盟副本等级")
public class GuildPlayerRequestDungeonLvl extends _ARPCBase<GuildPlayerRequestDungeonLvl_Req, GuildPlayerRequestDungeonLvl_Return>
{

	@Override
	protected GuildPlayerRequestDungeonLvl_Req createRequest()
	{
		return new GuildPlayerRequestDungeonLvl_Req();
	}

	@Override
	protected GuildPlayerRequestDungeonLvl_Return createResponse()
	{	
		return new GuildPlayerRequestDungeonLvl_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildPlayerRequestDungeonLvl.ordinal();
	}
}
