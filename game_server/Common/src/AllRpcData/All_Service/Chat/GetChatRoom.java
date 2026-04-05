package AllRpcData.All_Service.Chat;


import ALLRPC.Common.Chat.GetChatRoom_Req;
import ALLRPC.Common.Chat.GetChatRoom_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "获取聊天房间")
public class GetChatRoom extends _ARPCBase<GetChatRoom_Req, GetChatRoom_Return> implements _IAutoRegistHandler
{

    @Override
    protected GetChatRoom_Req createRequest()
    {
        return new GetChatRoom_Req();
    }

    @Override
    protected GetChatRoom_Return createResponse()
    {
        return new GetChatRoom_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.GetChatRoom.number();
    }
}
