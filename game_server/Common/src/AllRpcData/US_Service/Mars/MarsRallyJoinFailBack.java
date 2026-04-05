package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsRallyJoinFailBack_Req;
import ALLRPC.US.Mars.MarsRallyJoinFailBack_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 集结加入失败后遣返队伍")
public class MarsRallyJoinFailBack extends _ARPCBase<MarsRallyJoinFailBack_Req, MarsRallyJoinFailBack_Return>
{

	@Override
	protected MarsRallyJoinFailBack_Req createRequest()
	{
		return new MarsRallyJoinFailBack_Req();
	}

	@Override
	protected MarsRallyJoinFailBack_Return createResponse()
	{	
		return new MarsRallyJoinFailBack_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsRallyJoinFailBack.ordinal();
	}
}
