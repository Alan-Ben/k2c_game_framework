package AllRpcData.US_Service.Dinner;

import ALLRPC.US.Dinner.UsJoinDinner_Req;
import ALLRPC.US.Dinner.UsJoinDinner_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="加入宴会")
public class UsJoinDinner extends _ARPCBase<UsJoinDinner_Req, UsJoinDinner_Return>
{

	@Override
	protected UsJoinDinner_Req createRequest()
	{
		return new UsJoinDinner_Req();
	}

	@Override
	protected UsJoinDinner_Return createResponse()
	{	
		return new UsJoinDinner_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsJoinDinner.ordinal();
	}
}
