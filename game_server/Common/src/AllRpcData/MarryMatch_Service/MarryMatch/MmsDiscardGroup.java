package AllRpcData.MarryMatch_Service.MarryMatch;

import ALLRPC.MarryMatchServer.MarryMatch.MmsDiscardGroup_Req;
import ALLRPC.MarryMatchServer.MarryMatch.MmsDiscardGroup_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="销毁联姻池")
public class MmsDiscardGroup extends _ARPCBase<MmsDiscardGroup_Req, MmsDiscardGroup_Return>
{

	@Override
	protected MmsDiscardGroup_Req createRequest()
	{
		return new MmsDiscardGroup_Req();
	}

	@Override
	protected MmsDiscardGroup_Return createResponse()
	{	
		return new MmsDiscardGroup_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MmsDiscardGroup.ordinal();
	}
}
