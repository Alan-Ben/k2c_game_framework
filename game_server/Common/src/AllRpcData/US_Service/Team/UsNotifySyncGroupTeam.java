package AllRpcData.US_Service.Team;

import ALLRPC.US.Team.UsNotifySyncGroupTeam_Req;
import ALLRPC.US.Team.UsNotifySyncGroupTeam_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 通知US同步Group队伍成员数据")
public class UsNotifySyncGroupTeam extends _ARPCBase<UsNotifySyncGroupTeam_Req, UsNotifySyncGroupTeam_Return>
{

	@Override
	protected UsNotifySyncGroupTeam_Req createRequest()
	{
		return new UsNotifySyncGroupTeam_Req();
	}

	@Override
	protected UsNotifySyncGroupTeam_Return createResponse()
	{	
		return new UsNotifySyncGroupTeam_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsNotifySyncGroupTeam.ordinal();
	}
}
