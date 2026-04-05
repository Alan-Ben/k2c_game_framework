package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsGetPreDinnerInfoBySort_Req;
import ALLRPC.DinnerServer.Dinner.DnsGetPreDinnerInfoBySort_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取排序规则的前一个宴会数据")
public class DnsGetPreDinnerInfoBySort extends _ARPCBase<DnsGetPreDinnerInfoBySort_Req, DnsGetPreDinnerInfoBySort_Return>
{

	@Override
	protected DnsGetPreDinnerInfoBySort_Req createRequest()
	{
		return new DnsGetPreDinnerInfoBySort_Req();
	}

	@Override
	protected DnsGetPreDinnerInfoBySort_Return createResponse()
	{	
		return new DnsGetPreDinnerInfoBySort_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsGetPreDinnerInfoBySort.ordinal();
	}
}
