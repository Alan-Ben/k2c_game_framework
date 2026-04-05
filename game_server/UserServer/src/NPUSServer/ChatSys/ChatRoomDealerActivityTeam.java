package NPUSServer.ChatSys;

import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPEnum.ENPChatRoomType;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;

public class ChatRoomDealerActivityTeam implements _IChatRoomDealer
{
    @Override
    public ENPChatRoomType getRoomType()
    {
        return ENPChatRoomType.ACTIVITY_TEAM;
    }

    @Override
    public void getRoom(NPUserServer _server, long _roomTypeId, long _extId, _IGetChatRoomCallBackResult _callback)
    {
        _AActivityBase activity = _server.getCommActivityMgr().lookupActivity(_extId);
        if(null == activity)
        {
            _callback.onRunOver(ActivityErr.ACTIVITY_NOT_FOUND, 0L, 0, 0);
            return;
        }

        ActivityTeamDealer dealer = activity.getTeamDealer();
        if(null == dealer)
        {
            _callback.onRunOver(CommErr.OBJ_ERR, 0L, 0, 0);
            return;
        }
        
        dealer.getChatRoomId(_extId, (_result, _roomId, _chatServerType, _chatServerTypeId) ->
        {
            if(!_result.isSucc())
            {
                _callback.onRunOver(_result, 0L, 0, 0);
                return;
            }

            _callback.onRunOver(_result, _roomId, _chatServerType, _chatServerTypeId);
        });
    }
}
