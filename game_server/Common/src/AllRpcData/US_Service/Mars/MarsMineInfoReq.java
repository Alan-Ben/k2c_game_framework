package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineInfoReq_Req;
import ALLRPC.US.Mars.MarsMineInfoReq_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿结算数据")
public class MarsMineInfoReq extends _ARPCBase<MarsMineInfoReq_Req, MarsMineInfoReq_Return>
{

	@Override
	protected MarsMineInfoReq_Req createRequest()
	{
		return new MarsMineInfoReq_Req();
	}

	@Override
	protected MarsMineInfoReq_Return createResponse()
	{	
		return new MarsMineInfoReq_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineInfoReq.ordinal();
	}
}
