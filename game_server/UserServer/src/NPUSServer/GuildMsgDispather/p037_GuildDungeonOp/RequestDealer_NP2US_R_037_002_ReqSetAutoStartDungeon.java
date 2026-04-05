package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_002_ReqSetAutoStartDungeon;
import NPCommon.ErrMain.GuildErr;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildDungeon.GuildDungeonSetInfo;
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
public class RequestDealer_NP2US_R_037_002_ReqSetAutoStartDungeon extends _ATRequestDealer_GuildOp<GC2GS_037_002_ReqSetAutoStartDungeon>
{
    public RequestDealer_NP2US_R_037_002_ReqSetAutoStartDungeon(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_002_ReqSetAutoStartDungeon _msg)
    {
        //检查时间
        //检查分钟
        if(_msg.getMin() < 0 || _msg.getMin() > 59)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_AUTO_TIME_ERROR.getCode());
            return;
        }
        //检查小时
        if(_msg.getHour() < RefGeneral.Ref().guild_dungeon_allow_hour_range.first()
                || _msg.getHour() > RefGeneral.Ref().guild_dungeon_allow_hour_range.second())
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_AUTO_TIME_ERROR.getCode());
            return;
        }
        //如果是临界时间，则只能是准点时间
        if((_msg.getHour() == RefGeneral.Ref().guild_dungeon_allow_hour_range.first()
                || _msg.getHour() == RefGeneral.Ref().guild_dungeon_allow_hour_range.second())
                && _msg.getMin() != 0)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_AUTO_TIME_ERROR.getCode());
            return;
        }

        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //检查副本是否已经都解锁
        for(int i = 0; i < _msg.getAutoStartDungeonIdList().size(); i++)
        {
            GuildDungeonSetInfo setInfo = guildInfo.getDungeonMgr().getSetMgr().lookup(_msg.getAutoStartDungeonIdList().get(i));
            if(null == setInfo)
            {
                _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_UNLOCK.getCode());
                return;
            }

            if(!setInfo.isUnlock())
            {
                _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_UNLOCK.getCode());
                return;
            }
        }

        //设置权限
        GuildMemberInfo member = guildInfo.getMemberMgr().lookup(_committer.getCid());
        if(null == member)
        {
            _committer.commitFailRes(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
            return;
        }

        //检查权限
        if(!member.checkPermission(EGuildPermissionType.SET_PVE_AUTO_STAR))
        {
            _committer.commitFailRes(GuildErr.GUILD_POSITION_NOT_EXIST.getCode());
            return;
        }

        //设置自动开启时间
        guildInfo.getDungeonMgr().getGlobalSetInfo().cmdSetAutoStartTime(_msg.getHour(), _msg.getMin());

        //更新副本自动开启开关
        HashSet<Long> dungeonIdSet = new HashSet<>(_msg.getAutoStartDungeonIdList());
        guildInfo.getDungeonMgr().getSetMgr().updateAutoStartList(dungeonIdSet);

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_002_RetSetAutoStartDungeon());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.SET_PVE_AUTO_STAR;
    }

}
