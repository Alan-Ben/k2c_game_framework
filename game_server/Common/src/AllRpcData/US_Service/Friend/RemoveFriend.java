package AllRpcData.US_Service.Friend;

import ALLRPC.US.Friend.RemoveFriend_Req;
import ALLRPC.US.Friend.RemoveFriend_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 移除好友")
public class RemoveFriend extends _ARPCBase<RemoveFriend_Req, RemoveFriend_Return>
{

	@Override
	protected RemoveFriend_Req createRequest()
	{
		return new RemoveFriend_Req();
	}

	@Override
	protected RemoveFriend_Return createResponse()
	{	
		return new RemoveFriend_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.RemoveFriend.ordinal();
	}
}
