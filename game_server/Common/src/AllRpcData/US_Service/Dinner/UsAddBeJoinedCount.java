package AllRpcData.US_Service.Dinner;

import ALLRPC.US.Dinner.UsAddBeJoinedCount_Req;
import ALLRPC.US.Dinner.UsAddBeJoinedCount_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="玩家宴会交互记录")
public class UsAddBeJoinedCount extends _ARPCBase<UsAddBeJoinedCount_Req, UsAddBeJoinedCount_Return>
{

	@Override
	protected UsAddBeJoinedCount_Req createRequest()
	{
		return new UsAddBeJoinedCount_Req();
	}

	@Override
	protected UsAddBeJoinedCount_Return createResponse()
	{	
		return new UsAddBeJoinedCount_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsAddBeJoinedCount.ordinal();
	}
}
