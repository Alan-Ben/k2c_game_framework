package NPUSServer.NPUSUserMgr.PlayerChatRoomDealer;

import ChatSystem._AChatRoomInfo;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPEnum.ENPChatRoomType;
import NPUSServer.ChatSys.ChatRoomDealerMgr;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.ChatSys._IChatRoomDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class PlayerChatRoomUsDealer implements _IPlayerChatRoomDealer
{
    @Override
    public ENPChatRoomType getRoomType()
    {
        return ENPChatRoomType.US_SERVER;
    }

    @Override
    public void getRoom(NPUSUserData _userData, long _extId, _IGetChatRoomCallBackResult _callback)
    {
        //获取公会聊天房间处理对象
        _IChatRoomDealer dealer = ChatRoomDealerMgr.getInstance().getDealer(getRoomType().ordinal());
        if(null == dealer)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND, 0L, 0, 0);
            return;
        }

        dealer.getRoom(_userData.getUSServer(), _userData.getUSServer().getServerTypeId(), _extId, _callback);
    }

    @Override
    public void joinRoom(NPUSUserData _userData, long _extId, _ICallBackResultT<Long> _callback)
    {
        //检查玩家是否注册聊天用户
        ChatUserInfo chatUser = _userData.getUSServer().getChatUserMgr().lookupChatUser(_userData.getCid());
        if(null == chatUser)
        {
            _callback.onRunOver(ChatErr.CHAT_USER_NOT_FOUND, null);
            return;
        }

        _AChatRoomInfo room = _userData.getUSServer().getUSChatRoomInfo();
        if(null == room)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND, 0L);
            return;
        }

        room.JoinRoom(chatUser.toChatUser(), (_result, _roomId) ->
        {
            if(!_result.isSucc())
            {
                _callback.onRunOver(_result, 0L);
                return;
            }

            _callback.onRunOver(Result.SUCC, _roomId);
        });
    }

    @Override
    public void quitRoom(NPUSUserData _userData, long _extId, _ICallBackResult _callback)
    {
        //本服聊天不允许退出
        _callback.onRunOver(CommErr.PARAM_ERROR);
    }
}
