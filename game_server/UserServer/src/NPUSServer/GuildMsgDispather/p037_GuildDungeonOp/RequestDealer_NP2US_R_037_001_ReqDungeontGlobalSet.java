package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_001_ReqDungeontGlobalSet;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_001_ReqDungeontGlobalSet extends _ATRequestDealer_GuildOp<GC2GS_037_001_ReqDungeontGlobalSet>
{
    public RequestDealer_NP2US_R_037_001_ReqDungeontGlobalSet(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_001_ReqDungeontGlobalSet _msg)
    {
        if(null == _committer.getGuildInfo())
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_001_RetDungeontGlobalSet(_committer.getGuildInfo()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
