package NPUSServer.NPUSUserMgr.PlayerChatRoomDealer;

import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPEnum.ENPChatRoomType;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public interface _IPlayerChatRoomDealer
{
    /**
     * 获取聊天房间类型
     * @return
     */
    ENPChatRoomType getRoomType();

    /**
     * 根据用户数据获取对应的聊天房间ID
     * @param _userData
     * @param _extId
     * @param _callback
     */
    void getRoom(NPUSUserData _userData, long _extId, _IGetChatRoomCallBackResult _callback);

    /**
     * 加入聊天房间
     * @param _userData
     * @param _extId
     * @param _callback
     */
    void joinRoom(NPUSUserData _userData, long _extId, _ICallBackResultT<Long> _callback);

    /**
     * 退出聊天房间
     * @param _userData
     * @param _extId
     * @param _callback
     */
    void quitRoom(NPUSUserData _userData, long _extId, _ICallBackResult _callback);
}
