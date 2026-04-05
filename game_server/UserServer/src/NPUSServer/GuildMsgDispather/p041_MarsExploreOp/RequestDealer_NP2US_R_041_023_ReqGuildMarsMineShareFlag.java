package NPUSServer.GuildMsgDispather.p041_MarsExploreOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p041_MarsExploreOp.GC2GS_041_023_ReqGuildMarsMineShareFlag;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_041_023_ReqGuildMarsMineShareFlag extends _ATRequestDealer_GuildOp<GC2GS_041_023_ReqGuildMarsMineShareFlag>
{
    public RequestDealer_NP2US_R_041_023_ReqGuildMarsMineShareFlag(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_041_023_ReqGuildMarsMineShareFlag _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        getUSServer().sendMsgToGC(_committer.getCid(), US2GCWriter_041_MarsExploreOp.make_063_OnGuildMarsMineShareAdd(guildInfo));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
