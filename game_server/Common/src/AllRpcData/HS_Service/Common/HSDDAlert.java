package AllRpcData.HS_Service.Common;

import ALLRPC.HS.HSDDAlert_Req;
import ALLRPC.HS.HSDDAlert_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="发送钉钉预警")
public class HSDDAlert extends _ARPCBase<HSDDAlert_Req, HSDDAlert_Return>
{

	@Override
	protected HSDDAlert_Req createRequest()
	{
		return new HSDDAlert_Req();
	}

	@Override
	protected HSDDAlert_Return createResponse()
	{	
		return new HSDDAlert_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.HSDDAlert.ordinal();
	}
}
