package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_004_ReqUpgradeDungeonLvl;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildDungeon.GuildDungeonSetInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_004_ReqUpgradeDungeonLvl extends _ATRequestDealer_GuildOp<GC2GS_037_004_ReqUpgradeDungeonLvl>
{
    public RequestDealer_NP2US_R_037_004_ReqUpgradeDungeonLvl(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_004_ReqUpgradeDungeonLvl _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildDungeonSetInfo setInfo = guild.getDungeonMgr().getSetMgr().lookup(_msg.getDungeonId());
        if(null == setInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        if(!setInfo.isUnlock())
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_UNLOCK.getCode());
            return;
        }

        RefGuildDungeonLvl lvlRef = setInfo.getLvlRef();
        if(null == lvlRef)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_ERR.getCode());
            return;
        }

        //检查当前等级，避免被其他玩家升了等级
        long costWealth = setInfo.getLvlRef().upgrade_cost_guild_wealth;
        if(_msg.getCurLvl() != lvlRef.lvl)
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_LVL_UPDATED.getCode());
            return;
        }

        //检查下一个等级
        RefGuildDungeonLvl nextLvlRef = setInfo.getRef().getLevelMapMgr().getLevelData(_msg.getCurLvl() + 1);
        if(null == nextLvlRef)
        {
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //检查联盟财富
        if(costWealth > guild.getWealth())
        {
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_UPGRADE_LVL);

        //扣除联盟财富
        if(!guild.spendWealth(costWealth, context))
        {
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //升级操作
        Result result = setInfo.checkUpgradeLvl(lvlRef.lvl, context);
        if(!result.isSucc())
        {
            //更新失败，退回联盟财富
            NPPlayerContext returnContext = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_UPGRADE_FAIL);
            returnContext.setGuid(context.getGuid());
            guild.gainWealth(costWealth, returnContext);

            _committer.commitFailRes(result.getCode());
            return;
        }

        //更新玩家等级
        if(!setInfo.cmdUpgradeLvl(lvlRef, nextLvlRef, context))
        {
            //更新失败，退回联盟财富
            NPPlayerContext returnContext = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_UPGRADE_FAIL);
            returnContext.setGuid(context.getGuid());
            guild.gainWealth(costWealth, returnContext);

            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_SET_LVL_FAIL.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_004_RetUpgradeDungeonLvl(setInfo));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.USE_GUILD_WEALTH;
    }

}
