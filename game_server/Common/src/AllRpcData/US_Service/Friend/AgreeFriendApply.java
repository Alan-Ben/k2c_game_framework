package AllRpcData.US_Service.Friend;

import ALLRPC.US.Friend.AgreeFriendApply_Req;
import ALLRPC.US.Friend.AgreeFriendApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 同意好友申请")
public class AgreeFriendApply extends _ARPCBase<AgreeFriendApply_Req, AgreeFriendApply_Return>
{

	@Override
	protected AgreeFriendApply_Req createRequest()
	{
		return new AgreeFriendApply_Req();
	}

	@Override
	protected AgreeFriendApply_Return createResponse()
	{	
		return new AgreeFriendApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.AgreeFriendApply.ordinal();
	}
}
