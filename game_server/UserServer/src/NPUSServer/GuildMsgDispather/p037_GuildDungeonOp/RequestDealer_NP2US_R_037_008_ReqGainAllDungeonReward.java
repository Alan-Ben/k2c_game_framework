package NPUSServer.GuildMsgDispather.p037_GuildDungeonOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetGainItemInfo;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_008_ReqGainAllDungeonReward;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.ErrMain.GuildErr;
import NPCommon.NPCommon_ItemInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_037_008_ReqGainAllDungeonReward extends _ATRequestDealer_GuildOp<GC2GS_037_008_ReqGainAllDungeonReward>
{
    public RequestDealer_NP2US_R_037_008_ReqGainAllDungeonReward(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_037_008_ReqGainAllDungeonReward _msg)
    {
        GuildInfo guild = _committer.getGuildInfo();
        if(null == guild)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //计算奖励
        NPItemCollector collector = new NPItemCollector(0);
        guild.getDungeonMgr().getInstanceMgr().setGainAllMonsterReward(_committer.getCid(), collector);
        if(collector.isEmpty())
        {
            _committer.commitFailRes(GuildErr.GUILD_DUNGEON_REWARD_EMPTY.getCode());
            return;
        }

        GuildOp_RetGainItemInfo retInfo = new GuildOp_RetGainItemInfo();
        for(NPCommonCostItem item : collector.getAllItemList())
        {
            retInfo.getItemList().add(new NPCommon_ItemInfo(item.getItemType().ordinal(), item.getItemId(), item.getCount(), null));
        }

        //推送数据
        getUSServer().sendMsgToGC(_committer.getCid(), US2GCWriter_037_GuildDungeonOp.make_053_OnDungeonGainedRewardChg(guild, _committer.getCid()));

        //直接返回
        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
