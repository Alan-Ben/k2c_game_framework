package AllRpcData.US_Service.Child;

import ALLRPC.US.Child.GetServerPoolAdult_Req;
import ALLRPC.US.Child.GetServerPoolAdult_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 获取联姻池指定子嗣数据")
public class GetServerPoolAdult extends _ARPCBase<GetServerPoolAdult_Req, GetServerPoolAdult_Return>
{

	@Override
	protected GetServerPoolAdult_Req createRequest()
	{
		return new GetServerPoolAdult_Req();
	}

	@Override
	protected GetServerPoolAdult_Return createResponse()
	{	
		return new GetServerPoolAdult_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GetServerPoolAdult.ordinal();
	}
}
