package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetGainItemInfo;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_011_ReqGainDungeonReward;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;

public class MsgDealer_GC2GS_037_011_ReqGainDungeonReward extends NPUserMsgDealer<GC2GS_037_011_ReqGainDungeonReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_011_ReqGainDungeonReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_GAIN_REWARD);

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetGainItemInfo>(_commiter) {
                    @Override
                    protected GuildOp_RetGainItemInfo _createNewTmpObj() {
                        return new GuildOp_RetGainItemInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetGainItemInfo _retMsg) {
                        //领取奖励
                        userData.gainItemListP(_retMsg.getItemList(), context);

                        //通用弹框
                        if(!context.getCollector().isEmpty())
                        {
                            userData.sendMsgToGC(context.getCollector().toProto());
                        }

                        _commiter.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_011_RetGainDungeonReward());
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
