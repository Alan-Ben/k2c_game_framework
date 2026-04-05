package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineOccupyReq_Req;
import ALLRPC.US.Mars.MarsMineOccupyReq_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿结算数据")
public class MarsMineOccupyReq extends _ARPCBase<MarsMineOccupyReq_Req, MarsMineOccupyReq_Return>
{

	@Override
	protected MarsMineOccupyReq_Req createRequest()
	{
		return new MarsMineOccupyReq_Req();
	}

	@Override
	protected MarsMineOccupyReq_Return createResponse()
	{	
		return new MarsMineOccupyReq_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineOccupyReq.ordinal();
	}
}
