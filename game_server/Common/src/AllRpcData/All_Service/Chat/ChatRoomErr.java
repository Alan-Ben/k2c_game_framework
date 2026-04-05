package AllRpcData.All_Service.Chat;


import ALLRPC.Common.Chat.ChatRoomErr_Req;
import ALLRPC.Common.Chat.ChatRoomErr_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;
import RPC._IAutoRegistHandler;

@ARpc(comment = "聊天房间错误")
public class ChatRoomErr extends _ARPCBase<ChatRoomErr_Req, ChatRoomErr_Return> implements _IAutoRegistHandler
{

    @Override
    protected ChatRoomErr_Req createRequest()
    {
        return new ChatRoomErr_Req();
    }

    @Override
    protected ChatRoomErr_Return createResponse()
    {
        return new ChatRoomErr_Return();
    }

    @Override
    public int getClassId()
    {
        return ERpcClassName.ChatRoomErr.number();
    }
}
