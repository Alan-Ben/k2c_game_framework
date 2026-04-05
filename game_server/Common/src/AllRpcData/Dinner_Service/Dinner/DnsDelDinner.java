package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsDelDinner_Req;
import ALLRPC.DinnerServer.Dinner.DnsDelDinner_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="向跨服宴会池移除宴会数据")
public class DnsDelDinner extends _ARPCBase<DnsDelDinner_Req, DnsDelDinner_Return>
{

	@Override
	protected DnsDelDinner_Req createRequest()
	{
		return new DnsDelDinner_Req();
	}

	@Override
	protected DnsDelDinner_Return createResponse()
	{	
		return new DnsDelDinner_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsDelDinner.ordinal();
	}
}
