package AllRpcData.CS_Service.Common;

import ALLRPC.CS.Common.GetPHPParamList_Req;
import ALLRPC.CS.Common.GetPHPParamList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 从CS获取平台参数数据")
public class GetPHPParamList extends _ARPCBase<GetPHPParamList_Req, GetPHPParamList_Return>
{

	@Override
	protected GetPHPParamList_Req createRequest()
	{
		return new GetPHPParamList_Req();
	}

	@Override
	protected GetPHPParamList_Return createResponse()
	{	
		return new GetPHPParamList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GetPHPParamList.ordinal();
	}
}
