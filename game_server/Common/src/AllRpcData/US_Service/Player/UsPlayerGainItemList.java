package AllRpcData.US_Service.Player;

import ALLRPC.US.Player.UsPlayerGainItemList_Req;
import ALLRPC.US.Player.UsPlayerGainItemList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="玩家获得物品列表")
public class UsPlayerGainItemList extends _ARPCBase<UsPlayerGainItemList_Req, UsPlayerGainItemList_Return>
{

	@Override
	protected UsPlayerGainItemList_Req createRequest()
	{
		return new UsPlayerGainItemList_Req();
	}

	@Override
	protected UsPlayerGainItemList_Return createResponse()
	{	
		return new UsPlayerGainItemList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsPlayerGainItemList.ordinal();
	}
}
