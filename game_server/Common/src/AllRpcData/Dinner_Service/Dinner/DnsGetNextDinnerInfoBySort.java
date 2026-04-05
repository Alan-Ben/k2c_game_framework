package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsGetNextDinnerInfoBySort_Req;
import ALLRPC.DinnerServer.Dinner.DnsGetNextDinnerInfoBySort_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取排序规则的后一个宴会数据")
public class DnsGetNextDinnerInfoBySort extends _ARPCBase<DnsGetNextDinnerInfoBySort_Req, DnsGetNextDinnerInfoBySort_Return>
{

	@Override
	protected DnsGetNextDinnerInfoBySort_Req createRequest()
	{
		return new DnsGetNextDinnerInfoBySort_Req();
	}

	@Override
	protected DnsGetNextDinnerInfoBySort_Return createResponse()
	{	
		return new DnsGetNextDinnerInfoBySort_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsGetNextDinnerInfoBySort.ordinal();
	}
}
