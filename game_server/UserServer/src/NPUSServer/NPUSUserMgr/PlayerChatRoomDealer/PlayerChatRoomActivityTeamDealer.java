package NPUSServer.NPUSUserMgr.PlayerChatRoomDealer;

import AllRpcData.All_Service.Chat.JoinChatRoom;
import AllRpcData.All_Service.Chat.QuitChatRoom;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPEnum.ENPChatRoomType;
import NPUSServer.ChatSys.ChatRoomDealerMgr;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.ChatSys._IChatRoomDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import RPC._ARpcCallBack;

public class PlayerChatRoomActivityTeamDealer implements _IPlayerChatRoomDealer
{
    @Override
    public ENPChatRoomType getRoomType()
    {
        return ENPChatRoomType.ACTIVITY_TEAM;
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

        dealer.getRoom(_userData.getUSServer(), 0, _extId, _callback);
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

        //先获取房间，再执行加入房间操作
        getRoom(_userData, _extId, (_result, _roomId, _chatServerType, _chatServerTypeId) ->
        {
            if(!_result.isSucc())
            {
                _callback.onRunOver(_result, 0L);
                return;
            }

            JoinChatRoom rpc = new JoinChatRoom();
            rpc.req().setRoomId(_roomId);
            rpc.req().setChatUser(chatUser.toChatUser());

            _userData.getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<JoinChatRoom>()
            {
                @Override
                public void call_back(int _errCode, JoinChatRoom _rpc)
                {
                    if(_errCode > 0)
                    {
                        _callback.onRunOver(Result.failed(_errCode), 0L);
                        return;
                    }

                    _callback.onRunOver(Result.SUCC, _rpc.retObj().getRoomId());
                }
            });
        });
    }

    @Override
    public void quitRoom(NPUSUserData _userData, long _extId, _ICallBackResult _callback)
    {
        //先获取房间，再执行退出房间操作
        getRoom(_userData, _extId, (_result, _roomId, _chatServerType, _chatServerTypeId) ->
        {
            if(!_result.isSucc())
            {
                //聊天房间不存在，等同成功退出聊天房间
                if(_result.getCode() == ChatErr.CHAT_ROOM_NOT_FOUND.getCode()
                        || _result.getCode() == ChatErr.CHAT_ROOM_NOT_INITED.getCode())
                {
                    _callback.onRunOver(Result.SUCC);
                    return;
                }

                _callback.onRunOver(_result);
                return;
            }

            QuitChatRoom rpc = new QuitChatRoom();
            rpc.req().setRoomId(_roomId);
            rpc.req().setCid(_userData.getCid());

            _userData.getUSServer().rpc2crossTeam().request(rpc, new _ARpcCallBack<QuitChatRoom>()
            {
                @Override
                public void call_back(int _errCode, QuitChatRoom _rpc)
                {
                    if(_errCode > 0)
                    {
                        //聊天房间/用户不存在，等同成功退出聊天房间
                        if(_errCode == ChatErr.CHAT_ROOM_NOT_FOUND.getCode()
                                || _errCode == ChatErr.CHAT_ROOM_NOT_INITED.getCode()
                                || _errCode == ChatErr.CHAT_USER_NOT_FOUND.getCode())
                        {
                            _callback.onRunOver(Result.SUCC);
                            return;
                        }

                        _callback.onRunOver(Result.failed(_errCode));
                        return;
                    }

                    _callback.onRunOver(Result.SUCC);
                }
            });
        });
    }
}
