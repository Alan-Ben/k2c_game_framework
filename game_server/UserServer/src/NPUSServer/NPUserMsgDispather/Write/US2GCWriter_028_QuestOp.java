package NPUSServer.NPUserMsgDispather.Write;

import Common.QuestEnum.EDailyQuestType;
import Common.QuestObj.*;
import GS2GC.p028_QuestOp.*;
import NPCommon.CommonObj.NPItemCollector;

import java.util.ArrayList;

/**
 * @description: 028 协议writer
 * @author: mark
 * @date: 2022-04-08 11:58:05
 */
public class US2GCWriter_028_QuestOp
{

    public static GS2GC_028_001_RetStartQuest make_001_RetStartQuest()
    {
        return new GS2GC_028_001_RetStartQuest();
    }

    public static GS2GC_028_002_RetFinishQuest make_002_RetFinishQuest(long questId, long questStep, NPItemCollector _collector)
    {
        GS2GC_028_002_RetFinishQuest proto = new GS2GC_028_002_RetFinishQuest();
        proto.setQuestId(questId);
        proto.setQuestStepId(questStep);
        _collector.fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_028_003_RetDropQuest make_003_RetDropQuest()
    {
        return new GS2GC_028_003_RetDropQuest();
    }

    public static GS2GC_028_004_RetAddClientTargetCount make_004_RetAddClientTargetCount()
    {
        return new GS2GC_028_004_RetAddClientTargetCount();
    }

    public static GS2GC_028_010_RetFinishDailyQuest make_010_RetFinishDailyQuest(NPItemCollector _collector)
    {
        GS2GC_028_010_RetFinishDailyQuest proto = new GS2GC_028_010_RetFinishDailyQuest();
        _collector.fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_028_011_RetDrawActiveReward make_011_RetDrawActiveReward()
    {
        return new GS2GC_028_011_RetDrawActiveReward();
    }

    public static GS2GC_028_012_RetDailyQuestTryFresh make_012_RetDailyQuestTryFresh(DailyQuest_Group _dailyQuestGroupInfo)
    {
        GS2GC_028_012_RetDailyQuestTryFresh proto = new GS2GC_028_012_RetDailyQuestTryFresh();
        proto.setDailyquestInfo(_dailyQuestGroupInfo);

        return proto;
    }

    public static GS2GC_028_013_RetDailyQuestAKeyDrawFinishReward make_013_RetDailyQuestAKeyDrawFinishReward()
    {
        return new GS2GC_028_013_RetDailyQuestAKeyDrawFinishReward();
    }

    public static GS2GC_028_014_RetDailyQuestAKeyDrawActiveReward make_014_RetDailyQuestAKeyDrawActiveReward()
    {
        return new GS2GC_028_014_RetDailyQuestAKeyDrawActiveReward();
    }

    public static GS2GC_028_020_RetSystemQuestDrawReward make_020_RetSystemQuestDrawReward(NPItemCollector _collector)
    {
        GS2GC_028_020_RetSystemQuestDrawReward proto = new GS2GC_028_020_RetSystemQuestDrawReward();
        _collector.fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_028_021_RetSystemQuestAddClientCount make_021_RetSystemQuestAddClientCount()
    {
        return new GS2GC_028_021_RetSystemQuestAddClientCount();
    }

    public static GS2GC_028_050_OnPlayerQuestChg make_050_OnPlayerQuestChg(Quest_info _questProto)
    {
        GS2GC_028_050_OnPlayerQuestChg proto = new GS2GC_028_050_OnPlayerQuestChg();
        proto.setQuest(_questProto);
        return proto;
    }

    public static GS2GC_028_051_OnPlayerQuestStepCountChg make_051_OnPlayerQuestStepCountChg(long _questId, long _questStep, long _stepTargetId, long _targetCount)
    {
        GS2GC_028_051_OnPlayerQuestStepCountChg proto = new GS2GC_028_051_OnPlayerQuestStepCountChg();
        proto.setQuestId(_questId);
        proto.setQuestStep(_questStep);
        proto.setQuestTargetId(_stepTargetId);
        proto.setQuestTargetCount(_targetCount);
        return proto;
    }

    public static GS2GC_028_052_OnPlayerQuestRemove make_052_OnPlayerQuestRemove(long _questId)
    {
        GS2GC_028_052_OnPlayerQuestRemove proto = new GS2GC_028_052_OnPlayerQuestRemove();
        proto.setQuestId(_questId);
        return proto;
    }

    public static GS2GC_028_053_OnPlayerQuestCountChg make_053_OnPlayerQuestCountChg(Quest_Count _questCount)
    {
        GS2GC_028_053_OnPlayerQuestCountChg proto = new GS2GC_028_053_OnPlayerQuestCountChg();
        proto.setQuestCount(_questCount);
        return proto;
    }

    public static GS2GC_028_060_OnPlayerDailyQuestChg make_060_OnPlayerDailyQuestChg(EDailyQuestType _type, DailyQuest_Info _info)
    {

        GS2GC_028_060_OnPlayerDailyQuestChg proto = new GS2GC_028_060_OnPlayerDailyQuestChg();
        proto.setType(_type);
        proto.setQuest(_info);

        return proto;
    }

    public static GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg make_061_OnPlayerDailyQuestActiveRewardChg
            (EDailyQuestType _type, long _freshSerial, ArrayList<Long> _hasTakenRewardIdList)
    {

        GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg proto = new GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg();
        proto.setType(_type);
        proto.setRefreshSerial(_freshSerial);
        proto.getHasTakenActiveRewardRefIdList().addAll(_hasTakenRewardIdList);

        return proto;
    }

    public static GS2GC_028_070_OnPlayerSystemQuestChg make_070_OnPlayerSystemQuestChg(SystemQuest_Info _info)
    {
        GS2GC_028_070_OnPlayerSystemQuestChg proto = new GS2GC_028_070_OnPlayerSystemQuestChg();
        proto.setInfo(_info);
        return proto;
    }
}
