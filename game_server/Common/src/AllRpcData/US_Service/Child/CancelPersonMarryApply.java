package AllRpcData.US_Service.Child;

import ALLRPC.US.Child.CancelPersonMarryApply_Req;
import ALLRPC.US.Child.CancelPersonMarryApply_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 取消子嗣指定联姻请求")
public class CancelPersonMarryApply extends _ARPCBase<CancelPersonMarryApply_Req, CancelPersonMarryApply_Return>
{

	@Override
	protected CancelPersonMarryApply_Req createRequest()
	{
		return new CancelPersonMarryApply_Req();
	}

	@Override
	protected CancelPersonMarryApply_Return createResponse()
	{	
		return new CancelPersonMarryApply_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.CancelPersonMarryApply.ordinal();
	}
}
