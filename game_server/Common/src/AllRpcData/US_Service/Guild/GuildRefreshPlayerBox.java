package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildRefreshPlayerBox_Req;
import ALLRPC.US.Guild.GuildRefreshPlayerBox_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="刷新获取玩家宝箱")
public class GuildRefreshPlayerBox extends _ARPCBase<GuildRefreshPlayerBox_Req, GuildRefreshPlayerBox_Return>
{

	@Override
	protected GuildRefreshPlayerBox_Req createRequest()
	{
		return new GuildRefreshPlayerBox_Req();
	}

	@Override
	protected GuildRefreshPlayerBox_Return createResponse()
	{	
		return new GuildRefreshPlayerBox_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildRefreshPlayerBox.ordinal();
	}
}
