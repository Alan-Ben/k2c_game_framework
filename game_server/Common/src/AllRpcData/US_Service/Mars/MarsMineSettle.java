package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineSettle_Req;
import ALLRPC.US.Mars.MarsMineSettle_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿结算数据")
public class MarsMineSettle extends _ARPCBase<MarsMineSettle_Req, MarsMineSettle_Return>
{

	@Override
	protected MarsMineSettle_Req createRequest()
	{
		return new MarsMineSettle_Req();
	}

	@Override
	protected MarsMineSettle_Return createResponse()
	{	
		return new MarsMineSettle_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineSettle.ordinal();
	}
}
