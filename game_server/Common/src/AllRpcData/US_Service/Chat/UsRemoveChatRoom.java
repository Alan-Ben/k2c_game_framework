package AllRpcData.US_Service.Chat;

import ALLRPC.US.Chat.UsRemoveChatRoom_Req;
import ALLRPC.US.Chat.UsRemoveChatRoom_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 移除聊天房间")
public class UsRemoveChatRoom extends _ARPCBase<UsRemoveChatRoom_Req, UsRemoveChatRoom_Return>
{

	@Override
	protected UsRemoveChatRoom_Req createRequest()
	{
		return new UsRemoveChatRoom_Req();
	}

	@Override
	protected UsRemoveChatRoom_Return createResponse()
	{	
		return new UsRemoveChatRoom_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsRemoveChatRoom.ordinal();
	}
}
