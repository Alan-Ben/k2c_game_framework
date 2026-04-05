package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_048_ReqGuildMarsBattleReportList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 请求联盟火星矿战报列表
 * 转发至公会所在UserServer处理
 */
public class MsgDealer_GC2GS_032_048_ReqGuildMarsBattleReportList extends NPUserMsgDealer<GC2GS_032_048_ReqGuildMarsBattleReportList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_032_048_ReqGuildMarsBattleReportList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if (userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        // 转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _commiter,
                userData.getCid(),
                userData.getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
