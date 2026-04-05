package NPUSServer.ChatSys;

import AllRpcData.All_Service.Chat.GetChatRoom;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPChatRoomType;
import NPUSServer.NPUserServer;
import RPC._ARpcCallBack;
import WCGCommon.Enum.NPEnum;

public class ChatRoomDealerGuild implements _IChatRoomDealer
{
    @Override
    public ENPChatRoomType getRoomType()
    {
        return ENPChatRoomType.GUILD;
    }

    @Override
    public void getRoom(NPUserServer _server, long _roomTypeId, long _extId, _IGetChatRoomCallBackResult _callback)
    {
        long guildId = _roomTypeId;
        if(guildId <= 0)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND, 0L, 0, 0);
            return;
        }

        int usId = CommonFunc.parseServerTypeIdFromInstanced(guildId);
        if(usId <= 0)
        {
            _callback.onRunOver(CommErr.OBJ_ERR, 0L, 0, 0);
            return;
        }

        GetChatRoom rpc = new GetChatRoom();
        rpc.req().setRoomType(getRoomType().ordinal());
        rpc.req().setRoomTypeId(guildId);

        _server.rpc2us().requestTo(usId, rpc, new _ARpcCallBack<GetChatRoom>()
        {
            @Override
            public void call_back(int _errCode, GetChatRoom _rpc)
            {
                if(_errCode > 0)
                {
                    _callback.onRunOver(Result.failed(_errCode), 0L, 0, 0);
                    return;
                }

                _callback.onRunOver(Result.SUCC, _rpc.retObj().getRoomId(), NPEnum.EServerType.USER.ordinal(), usId);
            }
        });
    }
}
