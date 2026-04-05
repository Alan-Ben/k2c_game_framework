package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 聊天错误
 ****/
 
public class ChatErr implements _IErrHolder
{
    public static final Result CHAT_ROOM_NOT_FOUND = Result.constInit(210001,"聊天房间不存在");
    public static final Result CHAT_ROOM_NOT_INITED = Result.constInit(210002,"聊天房间未初始化完成");
    public static final Result CHAT_USER_NOT_FOUND = Result.constInit(210003,"聊天用户不存在");
    public static final Result CHAT_USER_JOIN_ROOM_FAIL = Result.constInit(210006,"聊天用户加入房间失败");
    public static final Result CHAT_USER_QUIT_ROOM_FAIL = Result.constInit(210007,"聊天用户退出房间失败");
    public static final Result CHAT_SEND_MSG_FAIL = Result.constInit(210008,"发送消息失败");
    public static final Result CHAT_FORBID = Result.constInit(210009,"禁言中");
}
