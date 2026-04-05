package NPUSServer.GuildMsgDispather.p041_MarsExploreOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p041_MarsExploreOp.GC2GS_041_017_ReqGuildMineShareList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_041_017_ReqGuildMineShareList extends _ATRequestDealer_GuildOp<GC2GS_041_017_ReqGuildMineShareList>
{
    public RequestDealer_NP2US_R_041_017_ReqGuildMineShareList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_041_017_ReqGuildMineShareList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildMemberInfo member = guildInfo.getMemberMgr().lookup(_committer.getCid());
        if(null == member)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        _committer.commitSucRes(
                US2GCWriter_041_MarsExploreOp.make_017_ReqGuildMineShareList(guildInfo.getMarsMineShareMgr().makeShareListProto()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
