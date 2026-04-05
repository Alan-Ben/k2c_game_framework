package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_037_ReqDrawConstructReward;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetIntInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Guild.RefGuildConstructReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

public class MsgDealer_GC2GS_032_037_ReqDrawConstructReward extends NPUserMsgDealer<GC2GS_032_037_ReqDrawConstructReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_037_ReqDrawConstructReward _msg)
    {
        //参数物品数量检查
        if(!_committer.getUserData().checkItemCount(_msg.getNum()))
        {
        	_committer.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }


        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetIntInfo>(_committer) {
                    @Override
                    protected GuildOp_RetIntInfo _createNewTmpObj() {
                        return new GuildOp_RetIntInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetIntInfo _msg)
                    {
                        //查询奖励配置
                        RefGuildConstructReward refReward = RefGuildConstructReward.getMgr().get(_msg.getNum());
                        if (refReward == null) {
                            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
                            return ;
                        }

                        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_GUILD_CONSTRUCT_REWARD);
                        //记录领取
                        Result result = _commiter.getUserData().getGuildComponent().recordGainConstructReward(_msg.getNum(), context);
                        if (!result.isSucc()){
                            _commiter.commitFailRes(result.getCode());
                            return ;
                        }

                        //领取奖励
                        _commiter.getUserData().gainItemList(refReward.reward_item_list, context);

                        //返回奖励
                        _commiter.getUserData().sendMsgToGC(context.getCollector().toProto());

                        //返回操作结果
                        _commiter.commitSucRes(US2GCWriter_032_GuildOp.make_037_RetDrawConstructReward());
                    }
                },
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
