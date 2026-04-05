package AllRpcData.MarryMatch_Service.MarryMatch;

import ALLRPC.MarryMatchServer.MarryMatch.MmsRemoveMatchItem_Req;
import ALLRPC.MarryMatchServer.MarryMatch.MmsRemoveMatchItem_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 移除联姻池请求数据")
public class MmsRemoveMatchItem extends _ARPCBase<MmsRemoveMatchItem_Req, MmsRemoveMatchItem_Return>
{

	@Override
	protected MmsRemoveMatchItem_Req createRequest()
	{
		return new MmsRemoveMatchItem_Req();
	}

	@Override
	protected MmsRemoveMatchItem_Return createResponse()
	{	
		return new MmsRemoveMatchItem_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MmsRemoveMatchItem.ordinal();
	}
}
