package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildDungeonEnum.EGuildDungeon_StartType;
import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_003_ReqStartDungeon;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_003_ReqStartDungeon extends _ATRequestDealer_GuildOp<GC2GS_037_003_ReqStartDungeon>
{
    public RequestDealer_NP2US_R_037_003_ReqStartDungeon(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_003_ReqStartDungeon _msg)
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

        //检查权限
        if(EGuildDungeon_StartType.ALLIANCE_WEALTH == _msg.getStartType())
        {
            if(!member.checkPermission(EGuildPermissionType.USE_GUILD_WEALTH))
            {
                _committer.commitFailRes(GuildErr.GUILD_POSITION_NOT_EXIST.getCode());
                return;
            }
        }
        else if(EGuildDungeon_StartType.COMMON_ITEM == _msg.getStartType())
        {
            if(!member.checkPermission(EGuildPermissionType.USE_ITEM_START_PVE))
            {
                _committer.commitFailRes(GuildErr.GUILD_POSITION_NOT_EXIST.getCode());
                return;
            }
        }
        else
        {
            _committer.commitFailRes(CommErr.PARAM_ERROR.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_START);

        Result result = guild.getDungeonMgr().cmdStart(_committer.getCid(), _msg.getStartType(), _msg.getDungeonId(), context);
        if(!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_003_RetStartDungeon());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
