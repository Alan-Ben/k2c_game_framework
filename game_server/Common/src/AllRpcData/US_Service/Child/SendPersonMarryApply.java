package AllRpcData.US_Service.Child;

import ALLRPC.US.Child.SendPersonMarryApply_Req;
import ALLRPC.US.Child.SendPersonMarryApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 发起针对玩家的子嗣联姻请求")
public class SendPersonMarryApply extends _ARPCBase<SendPersonMarryApply_Req, SendPersonMarryApply_Return>
{

	@Override
	protected SendPersonMarryApply_Req createRequest()
	{
		return new SendPersonMarryApply_Req();
	}

	@Override
	protected SendPersonMarryApply_Return createResponse()
	{	
		return new SendPersonMarryApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.SendPersonMarryApply.ordinal();
	}
}
