package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsMineAddPVPLog_Req;
import ALLRPC.US.Mars.MarsMineAddPVPLog_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 火星矿PVP战报跨服添加")
public class MarsMineAddPVPLog extends _ARPCBase<MarsMineAddPVPLog_Req, MarsMineAddPVPLog_Return>
{

	@Override
	protected MarsMineAddPVPLog_Req createRequest()
	{
		return new MarsMineAddPVPLog_Req();
	}

	@Override
	protected MarsMineAddPVPLog_Return createResponse()
	{	
		return new MarsMineAddPVPLog_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsMineAddPVPLog.ordinal();
	}
}
