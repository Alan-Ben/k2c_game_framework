package AllRpcData.US_Service.Team;

import ALLRPC.US.Team.UsNotifyRemoveGroup_Req;
import ALLRPC.US.Team.UsNotifyRemoveGroup_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 通知US移除Group数据（队伍解散时）")
public class UsNotifyRemoveGroup extends _ARPCBase<UsNotifyRemoveGroup_Req, UsNotifyRemoveGroup_Return>
{

	@Override
	protected UsNotifyRemoveGroup_Req createRequest()
	{
		return new UsNotifyRemoveGroup_Req();
	}

	@Override
	protected UsNotifyRemoveGroup_Return createResponse()
	{	
		return new UsNotifyRemoveGroup_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsNotifyRemoveGroup.ordinal();
	}
}
