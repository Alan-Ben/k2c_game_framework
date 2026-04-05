package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsGetDinnerInfoBySort_Req;
import ALLRPC.DinnerServer.Dinner.DnsGetDinnerInfoBySort_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取排序规则的当前宴会数据")
public class DnsGetDinnerInfoBySort extends _ARPCBase<DnsGetDinnerInfoBySort_Req, DnsGetDinnerInfoBySort_Return>
{

	@Override
	protected DnsGetDinnerInfoBySort_Req createRequest()
	{
		return new DnsGetDinnerInfoBySort_Req();
	}

	@Override
	protected DnsGetDinnerInfoBySort_Return createResponse()
	{	
		return new DnsGetDinnerInfoBySort_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsGetDinnerInfoBySort.ordinal();
	}
}
