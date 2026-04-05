package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_033_ReqGuildEntrust;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetGainItemInfo;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.ENpRewardShowType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

public class MsgDealer_GC2GS_032_033_ReqGuildEntrust extends NPUserMsgDealer<GC2GS_032_033_ReqGuildEntrust>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_033_ReqGuildEntrust _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DEAL_ENTRUST);
        //检查是否有足够的点数
        boolean consumeCdResult = _committer.getUserData().spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().deal_entrust_lazy_cd_id, 1, context);
        if (!consumeCdResult) {
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //累积次数
        _committer.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.GUILD_DEAL_ENTRUST_TIMES, 1, context);


        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetGainItemInfo>(_committer) {
                    @Override
                    protected GuildOp_RetGainItemInfo _createNewTmpObj() {
                        return new GuildOp_RetGainItemInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetGainItemInfo _msg)
                    {
                        //发送奖励
                        _commiter.getUserData().gainItemListP(_msg.getItemList(), context);

                        //发送奖励提示
                        _commiter.getUserData().sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

                        //返回操作结果（透传暴击倍数，大于1表示暴击）
                        _commiter.commitSucRes(US2GCWriter_032_GuildOp.make_033_RetGuildEntrust(_msg.getCritMul()));
                    }
                },
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
