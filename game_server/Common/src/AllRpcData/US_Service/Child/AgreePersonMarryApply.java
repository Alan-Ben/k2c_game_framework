package AllRpcData.US_Service.Child;

import ALLRPC.US.Child.AgreePersonMarryApply_Req;
import ALLRPC.US.Child.AgreePersonMarryApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 同意子嗣指定联姻请求")
public class AgreePersonMarryApply extends _ARPCBase<AgreePersonMarryApply_Req, AgreePersonMarryApply_Return>
{

	@Override
	protected AgreePersonMarryApply_Req createRequest()
	{
		return new AgreePersonMarryApply_Req();
	}

	@Override
	protected AgreePersonMarryApply_Return createResponse()
	{	
		return new AgreePersonMarryApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.AgreePersonMarryApply.ordinal();
	}
}
