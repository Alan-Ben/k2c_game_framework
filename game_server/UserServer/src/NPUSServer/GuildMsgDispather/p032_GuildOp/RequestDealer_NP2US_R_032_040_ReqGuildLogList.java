package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_040_ReqGuildLogList;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟日志
 */
public class RequestDealer_NP2US_R_032_040_ReqGuildLogList extends _ATRequestDealer_GuildOp<GC2GS_032_040_ReqGuildLogList>
{
    public RequestDealer_NP2US_R_032_040_ReqGuildLogList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_040_ReqGuildLogList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        _committer.commitSucRes(
                US2GCWriter_032_GuildOp.make_040_RetGuildLogList(guildInfo.getGuildLogMgr().makeProto(_msg.getLastDbId(), _msg.getNum())));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}