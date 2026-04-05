package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineOccupyResult_Req;
import ALLRPC.US.Mars.MarsMineOccupyResult_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿占领结果数据")
public class MarsMineOccupyResult extends _ARPCBase<MarsMineOccupyResult_Req, MarsMineOccupyResult_Return>
{

	@Override
	protected MarsMineOccupyResult_Req createRequest()
	{
		return new MarsMineOccupyResult_Req();
	}

	@Override
	protected MarsMineOccupyResult_Return createResponse()
	{	
		return new MarsMineOccupyResult_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineOccupyResult.ordinal();
	}
}
