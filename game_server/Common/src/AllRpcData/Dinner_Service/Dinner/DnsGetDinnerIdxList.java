package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsGetDinnerIdxList_Req;
import ALLRPC.DinnerServer.Dinner.DnsGetDinnerIdxList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取跨服池宴会索引数据列表")
public class DnsGetDinnerIdxList extends _ARPCBase<DnsGetDinnerIdxList_Req, DnsGetDinnerIdxList_Return>
{

	@Override
	protected DnsGetDinnerIdxList_Req createRequest()
	{
		return new DnsGetDinnerIdxList_Req();
	}

	@Override
	protected DnsGetDinnerIdxList_Return createResponse()
	{	
		return new DnsGetDinnerIdxList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsGetDinnerIdxList.ordinal();
	}
}
