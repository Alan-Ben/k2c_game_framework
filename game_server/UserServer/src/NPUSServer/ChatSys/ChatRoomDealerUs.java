package NPUSServer.ChatSys;

import ChatSystem._AChatRoomInfo;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPEnum.ENPChatRoomType;
import NPUSServer.NPUserServer;

public class ChatRoomDealerUs implements _IChatRoomDealer
{
    @Override
    public ENPChatRoomType getRoomType()
    {
        return ENPChatRoomType.US_SERVER;
    }

    @Override
    public void getRoom(NPUserServer _server, long _roomTypeId, long _extId, _IGetChatRoomCallBackResult _callback)
    {
        _AChatRoomInfo room = _server.getUSChatRoomInfo();
        if(null == room)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND, 0L, 0, 0);
            return;
        }

        if(room.getRoomSdkId() <= 0)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_INITED, 0L, 0, 0);
            return;
        }

        _callback.onRunOver(Result.SUCC, room.getRoomSdkId(), _server.getServerType(), _server.getServerTypeId());
    }
}
