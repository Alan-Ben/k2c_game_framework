package AllRpcData.Dinner_Service.Dinner;

import ALLRPC.DinnerServer.Dinner.DnsDiscardGroup_Req;
import ALLRPC.DinnerServer.Dinner.DnsDiscardGroup_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="解散跨服宴会池指定分组")
public class DnsDiscardGroup extends _ARPCBase<DnsDiscardGroup_Req, DnsDiscardGroup_Return>
{

	@Override
	protected DnsDiscardGroup_Req createRequest()
	{
		return new DnsDiscardGroup_Req();
	}

	@Override
	protected DnsDiscardGroup_Return createResponse()
	{	
		return new DnsDiscardGroup_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.DnsDiscardGroup.ordinal();
	}
}
