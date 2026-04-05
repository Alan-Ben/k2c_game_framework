package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsJoinDinner_Req;
import ALLRPC.DinnerServer.Dinner.DnsJoinDinner_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="玩家参加宴会")
public class DnsJoinDinner extends _ARPCBase<DnsJoinDinner_Req, DnsJoinDinner_Return>
{

	@Override
	protected DnsJoinDinner_Req createRequest()
	{
		return new DnsJoinDinner_Req();
	}

	@Override
	protected DnsJoinDinner_Return createResponse()
	{	
		return new DnsJoinDinner_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsJoinDinner.ordinal();
	}
}
