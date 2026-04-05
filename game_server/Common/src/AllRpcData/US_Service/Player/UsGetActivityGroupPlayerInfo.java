package AllRpcData.US_Service.Player;

import ALLRPC.US.Player.UsGetActivityGroupPlayerInfo_Req;
import ALLRPC.US.Player.UsGetActivityGroupPlayerInfo_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="向US查询指定活动群组下玩家的数据，返回序列化的玩家数据")
public class UsGetActivityGroupPlayerInfo extends _ARPCBase<UsGetActivityGroupPlayerInfo_Req, UsGetActivityGroupPlayerInfo_Return>
{

	@Override
	protected UsGetActivityGroupPlayerInfo_Req createRequest()
	{
		return new UsGetActivityGroupPlayerInfo_Req();
	}

	@Override
	protected UsGetActivityGroupPlayerInfo_Return createResponse()
	{	
		return new UsGetActivityGroupPlayerInfo_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsGetActivityGroupPlayerInfo.ordinal();
	}
}
