package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineLeave_Req;
import ALLRPC.US.Mars.MarsMineLeave_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿结算数据")
public class MarsMineLeave extends _ARPCBase<MarsMineLeave_Req, MarsMineLeave_Return>
{

	@Override
	protected MarsMineLeave_Req createRequest()
	{
		return new MarsMineLeave_Req();
	}

	@Override
	protected MarsMineLeave_Return createResponse()
	{	
		return new MarsMineLeave_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineLeave.ordinal();
	}
}
