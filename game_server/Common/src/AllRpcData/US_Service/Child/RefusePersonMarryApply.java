package AllRpcData.US_Service.Child;

import ALLRPC.US.Child.RefusePersonMarryApply_Req;
import ALLRPC.US.Child.RefusePersonMarryApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 拒绝子嗣指定联姻请求")
public class RefusePersonMarryApply extends _ARPCBase<RefusePersonMarryApply_Req, RefusePersonMarryApply_Return>
{

	@Override
	protected RefusePersonMarryApply_Req createRequest()
	{
		return new RefusePersonMarryApply_Req();
	}

	@Override
	protected RefusePersonMarryApply_Return createResponse()
	{	
		return new RefusePersonMarryApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.RefusePersonMarryApply.ordinal();
	}
}
