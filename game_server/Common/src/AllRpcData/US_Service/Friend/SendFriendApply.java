package AllRpcData.US_Service.Friend;

import ALLRPC.US.Friend.SendFriendApply_Req;
import ALLRPC.US.Friend.SendFriendApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 发送好友申请")
public class SendFriendApply extends _ARPCBase<SendFriendApply_Req, SendFriendApply_Return>
{

	@Override
	protected SendFriendApply_Req createRequest()
	{
		return new SendFriendApply_Req();
	}

	@Override
	protected SendFriendApply_Return createResponse()
	{	
		return new SendFriendApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.SendFriendApply.ordinal();
	}
}
