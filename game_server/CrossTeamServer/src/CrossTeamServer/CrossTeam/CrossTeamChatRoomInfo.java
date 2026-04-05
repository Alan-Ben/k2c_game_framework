package CrossTeamServer.CrossTeam;

import AllRpcData.US_Service.Chat.UsRemoveChatRoom;
import ChatSystem._AChatRoomInfo;
import CrossTeamServer.CrossTeamServer;
import NPCommon.Log.CommLog;
import NPEnum.ENPChatRoomType;
import RPC._ARpcCallBack;

import java.util.ArrayList;

public class CrossTeamChatRoomInfo extends _AChatRoomInfo
{
    public CrossTeamChatRoomInfo(long _teamId)
    {
        super(CrossTeamServer.getInstance(), ENPChatRoomType.ACTIVITY_TEAM, _teamId);
    }

    @Override
    protected void _onDiscard()
    {
        UsRemoveChatRoom rpc = new UsRemoveChatRoom();
        rpc.req().setRoomId(getRoomSdkId());

        ArrayList<Integer> userIdList = getUsIdList();
        for(int i = 0; i < userIdList.size(); i++)
        {
            CrossTeamServer server = (CrossTeamServer) getServer();
            int usId = userIdList.get(i);
            server.rpc2us().requestTo(usId, rpc, new _ARpcCallBack<UsRemoveChatRoom>()
            {
                @Override
                public void call_back(int _errCode, UsRemoveChatRoom _rpc)
                {
                    if(_errCode > 0)
                    {
                        CommLog.error("CrossTeamChatRoomInfo discard fail, rpc us:{} errCode:{}", usId, _errCode);
                    }
                }
            });
        }
    }
}
