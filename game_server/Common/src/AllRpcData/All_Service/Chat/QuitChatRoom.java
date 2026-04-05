package AllRpcData.All_Service.Chat;


import ALLRPC.Common.Chat.QuitChatRoom_Req;
import ALLRPC.Common.Chat.QuitChatRoom_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "退出聊天房间")
public class QuitChatRoom extends _ARPCBase<QuitChatRoom_Req, QuitChatRoom_Return> implements _IAutoRegistHandler
{

    @Override
    protected QuitChatRoom_Req createRequest()
    {
        return new QuitChatRoom_Req();
    }

    @Override
    protected QuitChatRoom_Return createResponse()
    {
        return new QuitChatRoom_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.QuitChatRoom.number();
    }
}
