package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_010_ReqGetDungeonLogList;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildDungeon.GuildDungeonInstanceInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_010_ReqGetDungeonLogList extends _ATRequestDealer_GuildOp<GC2GS_037_010_ReqGetDungeonLogList>
{
    public RequestDealer_NP2US_R_037_010_ReqGetDungeonLogList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_010_ReqGetDungeonLogList _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildDungeonInstanceInfo instance = guild.getDungeonMgr().getInstanceMgr().lookup(_msg.getId());
        if(null == instance)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_START.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_010_RetGetDungeonLogList(instance));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
