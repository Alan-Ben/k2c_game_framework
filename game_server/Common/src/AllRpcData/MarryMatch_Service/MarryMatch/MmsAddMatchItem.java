package AllRpcData.MarryMatch_Service.MarryMatch;

import ALLRPC.MarryMatchServer.MarryMatch.MmsAddMatchItem_Req;
import ALLRPC.MarryMatchServer.MarryMatch.MmsAddMatchItem_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="增加联姻池请求数据")
public class MmsAddMatchItem extends _ARPCBase<MmsAddMatchItem_Req, MmsAddMatchItem_Return>
{

	@Override
	protected MmsAddMatchItem_Req createRequest()
	{
		return new MmsAddMatchItem_Req();
	}

	@Override
	protected MmsAddMatchItem_Return createResponse()
	{	
		return new MmsAddMatchItem_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MmsAddMatchItem.ordinal();
	}
}
