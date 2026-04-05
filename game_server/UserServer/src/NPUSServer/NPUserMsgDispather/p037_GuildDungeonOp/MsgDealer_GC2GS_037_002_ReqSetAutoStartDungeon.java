package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import GC2GS.p037_GuildDungeonOp.GC2GS_037_002_ReqSetAutoStartDungeon;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_037_002_ReqSetAutoStartDungeon extends NPUserMsgDealer<GC2GS_037_002_ReqSetAutoStartDungeon>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_002_ReqSetAutoStartDungeon _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

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
