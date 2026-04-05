package AllRpcData.US_Service.Common;

import ALLRPC.US.Common.ExecPlayerGmCommand_Req;
import ALLRPC.US.Common.ExecPlayerGmCommand_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 执行玩家GM命令")
public class ExecPlayerGmCommand extends _ARPCBase<ExecPlayerGmCommand_Req, ExecPlayerGmCommand_Return>
{

	@Override
	protected ExecPlayerGmCommand_Req createRequest()
	{
		return new ExecPlayerGmCommand_Req();
	}

	@Override
	protected ExecPlayerGmCommand_Return createResponse()
	{	
		return new ExecPlayerGmCommand_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.ExecPlayerGmCommand.ordinal();
	}
}
