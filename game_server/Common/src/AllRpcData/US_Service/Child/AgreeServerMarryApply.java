package AllRpcData.US_Service.Child;

import ALLRPC.US.Child.AgreeServerMarryApply_Req;
import ALLRPC.US.Child.AgreeServerMarryApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 同意子嗣全服联姻请求")
public class AgreeServerMarryApply extends _ARPCBase<AgreeServerMarryApply_Req, AgreeServerMarryApply_Return>
{

	@Override
	protected AgreeServerMarryApply_Req createRequest()
	{
		return new AgreeServerMarryApply_Req();
	}

	@Override
	protected AgreeServerMarryApply_Return createResponse()
	{	
		return new AgreeServerMarryApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.AgreeServerMarryApply.ordinal();
	}
}
