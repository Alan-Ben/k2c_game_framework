package AllRpcData.All_Service.Chat;


import ALLRPC.Common.Chat.JoinChatRoom_Req;
import ALLRPC.Common.Chat.JoinChatRoom_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "加入聊天房间")
public class JoinChatRoom extends _ARPCBase<JoinChatRoom_Req, JoinChatRoom_Return> implements _IAutoRegistHandler
{

    @Override
    protected JoinChatRoom_Req createRequest()
    {
        return new JoinChatRoom_Req();
    }

    @Override
    protected JoinChatRoom_Return createResponse()
    {
        return new JoinChatRoom_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.JoinChatRoom.number();
    }
}
