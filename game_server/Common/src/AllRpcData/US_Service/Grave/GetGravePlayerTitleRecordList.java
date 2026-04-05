package AllRpcData.US_Service.Grave;

import ALLRPC.US.Grave.GetGravePlayerTitleRecordList_Req;
import ALLRPC.US.Grave.GetGravePlayerTitleRecordList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 获取指定玩家的杰出者的称号列表")
public class GetGravePlayerTitleRecordList extends _ARPCBase<GetGravePlayerTitleRecordList_Req, GetGravePlayerTitleRecordList_Return>
{

	@Override
	protected GetGravePlayerTitleRecordList_Req createRequest()
	{
		return new GetGravePlayerTitleRecordList_Req();
	}

	@Override
	protected GetGravePlayerTitleRecordList_Return createResponse()
	{	
		return new GetGravePlayerTitleRecordList_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GetGravePlayerTitleRecordList.ordinal();
	}
}
