package AllRpcData.All_Service;


import ALLRPC.Common.PushPHPParamList_Req;
import ALLRPC.Common.PushPHPParamList_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "推送平台参数")
public class PushPHPParamList extends _ARPCBase<PushPHPParamList_Req, PushPHPParamList_Return> implements _IAutoRegistHandler
{

    @Override
    protected PushPHPParamList_Req createRequest()
    {
        return new PushPHPParamList_Req();
    }

    @Override
    protected PushPHPParamList_Return createResponse()
    {
        return new PushPHPParamList_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.PushPHPParamList.number();
    }
}
