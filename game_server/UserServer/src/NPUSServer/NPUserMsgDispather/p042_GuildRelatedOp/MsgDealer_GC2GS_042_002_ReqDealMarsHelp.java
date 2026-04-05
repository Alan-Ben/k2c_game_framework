package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetIntInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_002_ReqDealMarsHelp;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import USLOGDB.OptBo.Opt042002GuildMarsDealHelpBO;

public class MsgDealer_GC2GS_042_002_ReqDealMarsHelp extends NPUserMsgDealer<GC2GS_042_002_ReqDealMarsHelp>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_002_ReqDealMarsHelp _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_MARS_HELP_DEAL);

        //此时转发消息开启
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetIntInfo>(_commiter) {
                    @Override
                    protected GuildOp_RetIntInfo _createNewTmpObj() {
                        return new GuildOp_RetIntInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetIntInfo _retMsg) {

                        //自动领取帮助奖励
                        long cdCount = userData.getFixedCdComponent().getItemCount(RefGeneral.Ref().guild_mars_help_deal_reward_fixed_cd_id);
                        long realCount = Math.min(_retMsg.getNum(), cdCount);
                        if(realCount > 0)
                        {
                            //扣除次数
                            if(!userData.getFixedCdComponent().spendItem(RefGeneral.Ref().guild_mars_help_deal_reward_fixed_cd_id, realCount, context))
                            {
                                _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                                return;
                            }

                            NPCommonCostItem gainItem = RefGeneral.Ref().guild_mars_help_deal_reward_item.duplicate();
                            gainItem.setCount(gainItem.getCount() * realCount);
                            userData.gainItem(gainItem, context);

                            //推送奖励数据
//        	                userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));
                        }

                        _commiter.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_002_RetDealMarsHelp(_retMsg.getNum(), realCount));

                        //日志数据
                        Opt042002GuildMarsDealHelpBO optBo = new Opt042002GuildMarsDealHelpBO();
                        optBo.setGuid(getUSServer().getBM(), userData.getGuildComponent().getGuildId());
                        optBo.setDealCount(getUSServer().getBM(), _retMsg.getNum());
                        _commiter.getUserData().logEvent(optBo, context);
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );

    }
}
