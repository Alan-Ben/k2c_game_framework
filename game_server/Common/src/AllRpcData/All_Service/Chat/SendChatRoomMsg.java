package AllRpcData.All_Service.Chat;


import ALLRPC.Common.Chat.SendChatRoomMsg_Req;
import ALLRPC.Common.Chat.SendChatRoomMsg_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "向聊天房间发消息")
public class SendChatRoomMsg extends _ARPCBase<SendChatRoomMsg_Req, SendChatRoomMsg_Return> implements _IAutoRegistHandler
{

    @Override
    protected SendChatRoomMsg_Req createRequest()
    {
        return new SendChatRoomMsg_Req();
    }

    @Override
    protected SendChatRoomMsg_Return createResponse()
    {
        return new SendChatRoomMsg_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.SendChatRoomMsg.number();
    }
}
