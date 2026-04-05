package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_012_ReqSetTagMonsterList;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildDungeon.GuildDungeonInstanceInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

import java.util.HashSet;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_012_ReqSetTagMonsterList extends _ATRequestDealer_GuildOp<GC2GS_037_012_ReqSetTagMonsterList>
{
    public RequestDealer_NP2US_R_037_012_ReqSetTagMonsterList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_012_ReqSetTagMonsterList _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //设置权限
        GuildMemberInfo member = guild.getMemberMgr().lookup(_committer.getCid());
        if(null == member)
        {
            _committer.commitFailRes(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
            return;
        }

        //检查副本
        GuildDungeonInstanceInfo instance = guild.getDungeonMgr().getInstanceMgr().lookup(_msg.getId());
        if(null == instance)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_START.getCode());
            return;
        }

        Result result = instance.getMonsterMgr().cmdSetTag(new HashSet<>(_msg.getMonsterIdList()));
        if(!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_012_RetSetTagMonsterList());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.SET_PVE_MONSTER_TAG;
    }

}
