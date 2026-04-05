package MGClient.Cmd.Cmds;

import GC2GS.p021_PlayerInfo.GC2GS_021_020_ReqDealAnecdoteRewardEvent;
import GC2GS.p021_PlayerInfo.GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward;
import GC2GS.p021_PlayerInfo.GC2GS_021_022_ReqDrawAnecdoteEarningsFinalReward;
import GC2GS.p021_PlayerInfo.GC2GS_021_040_ReqDealAnecdoteChoiceEvent;
import GS2GC.p021_PlayerInfo.GS2GC_021_020_RetDealAnecdoteRewardEvent;
import GS2GC.p021_PlayerInfo.GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward;
import GS2GC.p021_PlayerInfo.GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward;
import GS2GC.p021_PlayerInfo.GS2GC_021_040_RetDealAnecdoteChoiceEvent;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

@Commander(comment = "政务", name = "anecdote")
public class CmdAnecdote extends CmdBase
{
    @Command(comment = "处理奖励事件")
    public void dealReward(long _instanceId)
    {
        GC2GS_021_020_ReqDealAnecdoteRewardEvent proto = new GC2GS_021_020_ReqDealAnecdoteRewardEvent();
        proto.setInstanceId(_instanceId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_020_RetDealAnecdoteRewardEvent>(GS2GC_021_020_RetDealAnecdoteRewardEvent.class)
        {
            @Override
            public void handle(GS2GC_021_020_RetDealAnecdoteRewardEvent _response)
            {
            }
        });
    }

    @Command(comment = "处理赚速事件1")
    public void dealEarningsFirst(long _instanceId)
    {
        GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward proto = new GC2GS_021_021_ReqDrawAnecdoteEarningsProcessReward();
        proto.setInstanceId(_instanceId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward>(GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward.class)
        {
            @Override
            public void handle(GS2GC_021_021_RetDrawAnecdoteEarningsProcessReward _response)
            {
            }
        });
    }

    @Command(comment = "处理奖励事件2")
    public void dealEarningsFinal(long _instanceId)
    {
        GC2GS_021_022_ReqDrawAnecdoteEarningsFinalReward proto = new GC2GS_021_022_ReqDrawAnecdoteEarningsFinalReward();
        proto.setInstanceId(_instanceId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward>(GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward.class)
        {
            @Override
            public void handle(GS2GC_021_022_RetDrawAnecdoteEarningsFinalReward _response)
            {
            }
        });
    }

    @Command(comment = "处理选择事件")
    public void dealChoice(long _instanceId, long _optionId)
    {
        GC2GS_021_040_ReqDealAnecdoteChoiceEvent proto = new GC2GS_021_040_ReqDealAnecdoteChoiceEvent();
        proto.setInstanceId(_instanceId);
        proto.setOptionId(_optionId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_040_RetDealAnecdoteChoiceEvent>(GS2GC_021_040_RetDealAnecdoteChoiceEvent.class)
        {
            @Override
            public void handle(GS2GC_021_040_RetDealAnecdoteChoiceEvent _response)
            {
            }
        });
    }
}
