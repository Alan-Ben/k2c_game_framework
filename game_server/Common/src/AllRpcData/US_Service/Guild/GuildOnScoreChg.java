package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildCidScoreChg_Req;
import ALLRPC.US.Guild.GuildCidScoreChg_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="添加火星矿分享")
public class GuildOnScoreChg extends _ARPCBase<GuildCidScoreChg_Req, GuildCidScoreChg_Return>
{

	@Override
	protected GuildCidScoreChg_Req createRequest()
	{
		return new GuildCidScoreChg_Req();
	}

	@Override
	protected GuildCidScoreChg_Return createResponse()
	{	
		return new GuildCidScoreChg_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildOnScoreChg.ordinal();
	}
}
