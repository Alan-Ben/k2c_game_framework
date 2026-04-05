package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import GC2GS.p037_GuildDungeonOp.GC2GS_037_010_ReqGetDungeonLogList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_037_010_ReqGetDungeonLogList extends NPUserMsgDealer<GC2GS_037_010_ReqGetDungeonLogList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_010_ReqGetDungeonLogList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _commiter,
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
