package NPCommon.Util.CallBack;

import NPCommon.ErrMain.Result.Result;

@FunctionalInterface
public interface _IGetChatRoomCallBackResult
{
    /**
     * 获取聊天房间回调结果
     * @param _result
     * @param _roomId 聊天房间ID
     * @param _chatServerType 聊天房间所在服务器类型
     * @param _chatServerTypeId 聊天房间所在服务器ID
     */
    void onRunOver(Result _result, Long _roomId, Integer _chatServerType, Integer _chatServerTypeId);
}
