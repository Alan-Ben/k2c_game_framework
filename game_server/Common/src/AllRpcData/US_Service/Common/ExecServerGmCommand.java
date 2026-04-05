package AllRpcData.US_Service.Common;

import ALLRPC.US.Common.ExecServerGmCommand_Req;
import ALLRPC.US.Common.ExecServerGmCommand_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 执行服务器GM命令")
public class ExecServerGmCommand extends _ARPCBase<ExecServerGmCommand_Req, ExecServerGmCommand_Return>
{

	@Override
	protected ExecServerGmCommand_Req createRequest()
	{
		return new ExecServerGmCommand_Req();
	}

	@Override
	protected ExecServerGmCommand_Return createResponse()
	{	
		return new ExecServerGmCommand_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.ExecServerGmCommand.ordinal();
	}
}
