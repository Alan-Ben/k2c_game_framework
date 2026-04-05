package MGClient.Cmd.Cmds;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_005_ReqSevenDayGoalsDrawReward;
import GC2GS.p033_SimpleActivityOp.GC2GS_033_006_ReqSevenDayGoalsDrawStepReward;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_005_RetSevenDayGoalsDrawReward;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_006_RetSevenDayGoalsDrawStepReward;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;
import NPCommon.Log.CommLog;

@Commander(comment = "七日任务相关", name = "sevenGoals")
public class CmdSevenDayGoals extends CmdBase
{
    @Command(comment = "领取任务奖励[奖励id]")
    public void drawReward(long _refId)
    {
        GC2GS_033_005_ReqSevenDayGoalsDrawReward proto = new GC2GS_033_005_ReqSevenDayGoalsDrawReward();
        proto.setRefId(_refId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_033_005_RetSevenDayGoalsDrawReward>(GS2GC_033_005_RetSevenDayGoalsDrawReward.class)
        {
            @Override
            public void handle(GS2GC_033_005_RetSevenDayGoalsDrawReward _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }

    @Command(comment = "领取阶段奖励[奖励id]")
    public void drawStepReward(long _refId)
    {
        GC2GS_033_006_ReqSevenDayGoalsDrawStepReward proto = new GC2GS_033_006_ReqSevenDayGoalsDrawStepReward();
        proto.setRefId(_refId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_033_006_RetSevenDayGoalsDrawStepReward>(GS2GC_033_006_RetSevenDayGoalsDrawStepReward.class)
        {
            @Override
            public void handle(GS2GC_033_006_RetSevenDayGoalsDrawStepReward _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }
}
