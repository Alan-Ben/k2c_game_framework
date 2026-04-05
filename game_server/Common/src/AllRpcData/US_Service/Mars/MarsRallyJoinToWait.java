package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsRallyJoinToWait_Req;
import ALLRPC.US.Mars.MarsRallyJoinToWait_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 集结加入-行军到达后切换等待集结状态")
public class MarsRallyJoinToWait extends _ARPCBase<MarsRallyJoinToWait_Req, MarsRallyJoinToWait_Return>
{

	@Override
	protected MarsRallyJoinToWait_Req createRequest()
	{
		return new MarsRallyJoinToWait_Req();
	}

	@Override
	protected MarsRallyJoinToWait_Return createResponse()
	{	
		return new MarsRallyJoinToWait_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsRallyJoinToWait.ordinal();
	}
}
