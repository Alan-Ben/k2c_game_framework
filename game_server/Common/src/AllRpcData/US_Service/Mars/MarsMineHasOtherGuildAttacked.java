package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineHasOtherGuildAttacked_Req;
import ALLRPC.US.Mars.MarsMineHasOtherGuildAttacked_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿结算数据")
public class MarsMineHasOtherGuildAttacked extends _ARPCBase<MarsMineHasOtherGuildAttacked_Req, MarsMineHasOtherGuildAttacked_Return>
{

	@Override
	protected MarsMineHasOtherGuildAttacked_Req createRequest()
	{
		return new MarsMineHasOtherGuildAttacked_Req();
	}

	@Override
	protected MarsMineHasOtherGuildAttacked_Return createResponse()
	{	
		return new MarsMineHasOtherGuildAttacked_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineHasOtherGuildAttacked.ordinal();
	}
}
