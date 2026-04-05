package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_007_ReqDamageRank;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_007_ReqDamageRank extends _ATRequestDealer_GuildOp<GC2GS_037_007_ReqDamageRank>
{
    public RequestDealer_NP2US_R_037_007_ReqDamageRank(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_007_ReqDamageRank _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        int page = _msg.getPage();
        int pageNum = Math.min(_msg.getPageNum(), 100);

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_007_RetDamageRank(_committer.getCid(), page, pageNum, guild));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
