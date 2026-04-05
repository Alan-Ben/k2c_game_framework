package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetGainItemInfo;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_011_ReqGainDungeonReward;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildDungeon.GuildDungeonInstanceInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_011_ReqGainDungeonReward extends _ATRequestDealer_GuildOp<GC2GS_037_011_ReqGainDungeonReward>
{
    public RequestDealer_NP2US_R_037_011_ReqGainDungeonReward(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_011_ReqGainDungeonReward _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //检查副本
        GuildDungeonInstanceInfo instance = guild.getDungeonMgr().getInstanceMgr().lookup(_msg.getId());
        if(null == instance)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_START.getCode());
            return;
        }

        //构造返回信息
        GuildOp_RetGainItemInfo retInfo = new GuildOp_RetGainItemInfo();

        Result result = instance.getMonsterMgr().cmdSetGainedReward(_committer.getCid(), _msg.getMonsterId(), retInfo.getItemList());
        if(!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //推送数据
        getUSServer().sendMsgToGC(_committer.getCid(), US2GCWriter_037_GuildDungeonOp.make_053_OnDungeonGainedRewardChg(guild, _committer.getCid()));

        //返回数据
        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
