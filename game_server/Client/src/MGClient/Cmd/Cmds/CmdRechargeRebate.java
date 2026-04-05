package MGClient.Cmd.Cmds;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_007_ReqRechargeRebateInfo;
import GC2GS.p033_SimpleActivityOp.GC2GS_033_008_ReqRechargeRebateDrawReward;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

@Commander(comment = "充值返利相关", name = "rechargeRebate")
public class CmdRechargeRebate extends CmdBase
{
    @Command(comment = "获取充值返利信息[活动实例id]")
    public void info(long _activityInstanceId)
    {
        GC2GS_033_007_ReqRechargeRebateInfo proto = new GC2GS_033_007_ReqRechargeRebateInfo();
        proto.setActivityInstanceId(_activityInstanceId);
        getOwner().sendGameMsg(proto);
    }

    @Command(comment = "领取充值返利奖励[活动实例id][返利组id][档位id]")
    public void drawReward(long _activityInstanceId, long _groupId, long _stepId)
    {
        GC2GS_033_008_ReqRechargeRebateDrawReward proto = new GC2GS_033_008_ReqRechargeRebateDrawReward();
        proto.setActivityInstanceId(_activityInstanceId);
        proto.setGroupId(_groupId);
        proto.setStepId(_stepId);
        getOwner().sendGameMsg(proto);
    }
}
