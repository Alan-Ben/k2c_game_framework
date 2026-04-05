package MGClient.Cmd.Cmds;

import Common.QuestEnum.EDailyQuestType;
import GC2GS.p002_InitOp.GC2GS_002_046_ReqDailyQuest;
import GC2GS.p028_QuestOp.GC2GS_028_010_ReqFinishDailyQuest;
import GC2GS.p028_QuestOp.GC2GS_028_011_ReqDrawActiveReward;
import GC2GS.p028_QuestOp.GC2GS_028_012_ReqDailyQuestTryFresh;
import GS2GC.p002_InitOp.GS2GC_002_046_RetDailyQuest;
import GS2GC.p028_QuestOp.GS2GC_028_010_RetFinishDailyQuest;
import GS2GC.p028_QuestOp.GS2GC_028_011_RetDrawActiveReward;
import GS2GC.p028_QuestOp.GS2GC_028_012_RetDailyQuestTryFresh;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "任务命令", name = "quest")
public class CmdQuest extends CmdBase
{
    @Command(comment = "日常任务信息")
    public void info()
    {
        GC2GS_002_046_ReqDailyQuest proto = new GC2GS_002_046_ReqDailyQuest();

        getOwner().request(proto, new _AClientRequestHandler<GS2GC_002_046_RetDailyQuest>(GS2GC_002_046_RetDailyQuest.class)
        {

            @Override
            public void handle(GS2GC_002_046_RetDailyQuest _response)
            {

            }
        });
    }

    @Command(comment = "完成日常任务[任务id][刷新序列号]")
    public void finish(long _questId, long _refreshSerial)
    {
        GC2GS_028_010_ReqFinishDailyQuest proto = new GC2GS_028_010_ReqFinishDailyQuest();
        proto.setRefreshSerial(_refreshSerial);
        proto.setQuestId(_questId);

        getOwner().request(proto, new _AClientRequestHandler<GS2GC_028_010_RetFinishDailyQuest>(GS2GC_028_010_RetFinishDailyQuest.class)
        {

            @Override
            public void handle(GS2GC_028_010_RetFinishDailyQuest _response)
            {

            }
        });
    }

    @Command(comment = "领取活跃度奖励[刷新序列号][rewardId]")
    public void drawActive(long _refreshSerial, long _rewardId)
    {
        GC2GS_028_011_ReqDrawActiveReward proto = new GC2GS_028_011_ReqDrawActiveReward();
        proto.setRefreshSerial(_refreshSerial);
        proto.setActiveRewardId(_rewardId);

        getOwner().request(proto, new _AClientRequestHandler<GS2GC_028_011_RetDrawActiveReward>(GS2GC_028_011_RetDrawActiveReward.class)
        {

            @Override
            public void handle(GS2GC_028_011_RetDrawActiveReward _response)
            {

            }
        });
    }

    @Command(comment = "尝试刷新任务数据[任务类型][刷新序列号]")
    public void tryFresh(int _type, long _refreshSerial)
    {
        GC2GS_028_012_ReqDailyQuestTryFresh proto = new GC2GS_028_012_ReqDailyQuestTryFresh();
        proto.setRefreshSerial(_refreshSerial);
        proto.setType(EDailyQuestType.EDailyQuestType_FromInt(_type));
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_028_012_RetDailyQuestTryFresh>(GS2GC_028_012_RetDailyQuestTryFresh.class)
        {

            @Override
            public void handle(GS2GC_028_012_RetDailyQuestTryFresh _response)
            {

            }
        });
    }
}
