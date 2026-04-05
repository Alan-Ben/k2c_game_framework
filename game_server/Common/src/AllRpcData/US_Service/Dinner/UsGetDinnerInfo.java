package AllRpcData.US_Service.Dinner;

import ALLRPC.US.Dinner.UsGetDinnerInfo_Req;
import ALLRPC.US.Dinner.UsGetDinnerInfo_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取宴会详细数据")
public class UsGetDinnerInfo extends _ARPCBase<UsGetDinnerInfo_Req, UsGetDinnerInfo_Return>
{

	@Override
	protected UsGetDinnerInfo_Req createRequest()
	{
		return new UsGetDinnerInfo_Req();
	}

	@Override
	protected UsGetDinnerInfo_Return createResponse()
	{	
		return new UsGetDinnerInfo_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsGetDinnerInfo.ordinal();
	}
}
