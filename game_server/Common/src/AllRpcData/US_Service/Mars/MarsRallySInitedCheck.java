package AllRpcData.US_Service.Mars;

import ALLRPC.US.Mars.MarsRallySInitedCheck_Req;
import ALLRPC.US.Mars.MarsRallySInitedCheck_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 启动阶段检查集结与成员是否存在")
public class MarsRallySInitedCheck extends _ARPCBase<MarsRallySInitedCheck_Req, MarsRallySInitedCheck_Return>
{

	@Override
	protected MarsRallySInitedCheck_Req createRequest()
	{
		return new MarsRallySInitedCheck_Req();
	}

	@Override
	protected MarsRallySInitedCheck_Return createResponse()
	{	
		return new MarsRallySInitedCheck_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.MarsRallySInitedCheck.ordinal();
	}
}
