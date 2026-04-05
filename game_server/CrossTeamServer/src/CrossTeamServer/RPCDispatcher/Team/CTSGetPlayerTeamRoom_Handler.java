package CrossTeamServer.RPCDispatcher.Team;

import AllRpcData.CrossTeam_Service.Team.CTSGetPlayerTeamRoom;
import CrossTeamServer.CrossTeam.CrossGroup;
import CrossTeamServer.CrossTeam.CrossGroupMgr;
import CrossTeamServer.CrossTeam.CrossTeamInfo;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CrossTeamErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：获取玩家的队伍聊天房间ID
 */
public class CTSGetPlayerTeamRoom_Handler extends RpcRequestHandler<CTSGetPlayerTeamRoom>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, CTSGetPlayerTeamRoom _rpc)
    {
        CrossGroup group = CrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
        if(null == group)
        {
            _rpc.commitFail(CrossTeamErr.GROUP_NOT_FOUND.getCode());
            return;
        }

        CrossTeamInfo team = group.getTeamMgr().getPlayerTeam(_rpc.req().getCid());
        if(null == team)
        {
            _rpc.commitFail(CrossTeamErr.TEAM_NOT_FOUND.getCode());
            return;
        }

        if(team.getChatRoom().getRoomSdkId() <= 0)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_INITED.getCode());
            return;
        }

        _rpc.retObj().setRoomId(team.getChatRoom().getRoomSdkId());
        _rpc.commit();
    }
}
