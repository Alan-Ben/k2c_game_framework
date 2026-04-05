package AllRpcData.All_Service;


import ALLRPC.Common.AllExecGmCommand_Req;
import ALLRPC.Common.AllExecGmCommand_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "在所有服务器上执行GM命令")
public class AllExecGmCommand extends _ARPCBase<AllExecGmCommand_Req, AllExecGmCommand_Return> implements _IAutoRegistHandler
{

    @Override
    protected AllExecGmCommand_Req createRequest()
    {
        return new AllExecGmCommand_Req();
    }

    @Override
    protected AllExecGmCommand_Return createResponse()
    {
        return new AllExecGmCommand_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.AllExecGmCommand.number();
    }
}
