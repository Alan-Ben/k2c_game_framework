package AllRpcData.US_Service.Dinner;

import ALLRPC.US.Dinner.UsGetDinnerIdx_Req;
import ALLRPC.US.Dinner.UsGetDinnerIdx_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取宴会索引数据")
public class UsGetDinnerIdx extends _ARPCBase<UsGetDinnerIdx_Req, UsGetDinnerIdx_Return>
{

	@Override
	protected UsGetDinnerIdx_Req createRequest()
	{
		return new UsGetDinnerIdx_Req();
	}

	@Override
	protected UsGetDinnerIdx_Return createResponse()
	{	
		return new UsGetDinnerIdx_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsGetDinnerIdx.ordinal();
	}
}
