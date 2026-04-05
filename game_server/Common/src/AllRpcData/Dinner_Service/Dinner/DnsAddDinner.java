package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsAddDinner_Req;
import ALLRPC.DinnerServer.Dinner.DnsAddDinner_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="向跨服宴会池增加宴会数据")
public class DnsAddDinner extends _ARPCBase<DnsAddDinner_Req, DnsAddDinner_Return>
{

	@Override
	protected DnsAddDinner_Req createRequest()
	{
		return new DnsAddDinner_Req();
	}

	@Override
	protected DnsAddDinner_Return createResponse()
	{	
		return new DnsAddDinner_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsAddDinner.ordinal();
	}
}
