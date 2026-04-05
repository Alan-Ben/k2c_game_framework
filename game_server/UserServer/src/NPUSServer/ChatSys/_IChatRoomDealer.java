package NPUSServer.ChatSys;

import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPEnum.ENPChatRoomType;
import NPUSServer.NPUserServer;

public interface _IChatRoomDealer
{
    /**
     * 获取聊天房间类型
     * @return
     */
    ENPChatRoomType getRoomType();

    /**
     * 获取聊天房间信息
     * @param _roomTypeId
     * @param _extId
     * @param _callback
     */
    void getRoom(NPUserServer _server, long _roomTypeId, long _extId, _IGetChatRoomCallBackResult _callback);
}
