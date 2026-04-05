package AllRpcData.MarryMatch_Service.MarryMatch;

import ALLRPC.MarryMatchServer.MarryMatch.MmsUploadMatchItemList_Req;
import ALLRPC.MarryMatchServer.MarryMatch.MmsUploadMatchItemList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="增加联姻池请求数据")
public class MmsUploadMatchItemList extends _ARPCBase<MmsUploadMatchItemList_Req, MmsUploadMatchItemList_Return>
{

	@Override
	protected MmsUploadMatchItemList_Req createRequest()
	{
		return new MmsUploadMatchItemList_Req();
	}

	@Override
	protected MmsUploadMatchItemList_Return createResponse()
	{	
		return new MmsUploadMatchItemList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MmsUploadMatchItemList.ordinal();
	}
}
