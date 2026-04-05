package AllRpcData.MarryMatch_Service.MarryMatch;

import ALLRPC.MarryMatchServer.MarryMatch.MmsGetMatchItemList_Req;
import ALLRPC.MarryMatchServer.MarryMatch.MmsGetMatchItemList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="获取符合匹配要求的子嗣基础数据列表")
public class MmsGetMatchItemList extends _ARPCBase<MmsGetMatchItemList_Req, MmsGetMatchItemList_Return>
{

	@Override
	protected MmsGetMatchItemList_Req createRequest()
	{
		return new MmsGetMatchItemList_Req();
	}

	@Override
	protected MmsGetMatchItemList_Return createResponse()
	{	
		return new MmsGetMatchItemList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MmsGetMatchItemList.ordinal();
	}
}
