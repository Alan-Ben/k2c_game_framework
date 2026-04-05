package AllRpcData.US_Service.Grave;

import ALLRPC.US.Grave.AddGraveNewInfo_Req;
import ALLRPC.US.Grave.AddGraveNewInfo_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 增加新晋杰出者")
public class AddGraveNewInfo extends _ARPCBase<AddGraveNewInfo_Req, AddGraveNewInfo_Return>
{

	@Override
	protected AddGraveNewInfo_Req createRequest()
	{
		return new AddGraveNewInfo_Req();
	}

	@Override
	protected AddGraveNewInfo_Return createResponse()
	{	
		return new AddGraveNewInfo_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.AddGraveNewInfo.ordinal();
	}
}
