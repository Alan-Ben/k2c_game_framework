package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_062_ReqQueryRallyInfo;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Rally.GuildRallyInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

/**
 * 查询单个集结信息（Guild侧处理入口）
 */
public class RequestDealer_NP2US_R_042_062_ReqQueryRallyInfo extends _ATRequestDealer_GuildOp<GC2GS_042_062_ReqQueryRallyInfo>
{
    public RequestDealer_NP2US_R_042_062_ReqQueryRallyInfo(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_062_ReqQueryRallyInfo _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if (guildInfo == null)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        ResultOne<GuildRallyInfo<?>> result = guildInfo.getRallyMgr().queryRallyInfo(_msg.getRallyId());
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_062_RetQueryRallyInfo(result.getData()));
    }

    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}


