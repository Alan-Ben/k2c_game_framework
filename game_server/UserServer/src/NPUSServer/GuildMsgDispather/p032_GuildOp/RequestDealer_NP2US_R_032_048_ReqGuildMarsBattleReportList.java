package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_048_ReqGuildMarsBattleReportList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟火星矿战报列表
 * 从公会对象中读取战报数据并返回给客户端
 */
public class RequestDealer_NP2US_R_032_048_ReqGuildMarsBattleReportList extends _ATRequestDealer_GuildOp<GC2GS_032_048_ReqGuildMarsBattleReportList>
{
    public RequestDealer_NP2US_R_032_048_ReqGuildMarsBattleReportList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_048_ReqGuildMarsBattleReportList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if (null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_048_RetGuildMarsBattleReportList(
                guildInfo.getMarsMineBattleReportMgr().makeReportListProto()));
    }

    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}
