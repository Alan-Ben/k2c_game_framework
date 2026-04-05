package NPUSServer.GMCommand.Cmds;

import Common.QuestEnum.EQuestType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "主线任务相关命令", name = "mainQuest")
public class CmdMainQuest extends UsCmdBase
{
    @ACommand(comment = "设置主线任务[任务ID]")
    public String setMainQuest(long _questId)
    {
        RefQuest ref = RefQuest.getMgr().get(_questId);
        if (null == ref) return "fail, not quest ref, " + _questId;

        if (ref.quest_type != EQuestType.MAIN) return "fail, not main quest, " + _questId;

        boolean res = getOwner().getQuestComponent().gmSetMainQuest(ref, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "设置主线任务到指定步骤[任务ID][任务步骤ID]")
    public String setQuestStep(long _questId, long _step)
    {
        RefQuest ref = RefQuest.getMgr().get(_questId);
        if (null == ref)
        {
            return "fail, not find quest, quest:" + _questId;
        }

        RefQuestStep stepRef = RefQuestStep.getMgr().get(_step);
        if (null == stepRef)
        {
            return "fail, not find quest-step, quest:" + _questId + ", step:" + _step;
        }
        if (!ref.listStep.contains(stepRef))
        {
            return "fail, not contanins step, quest:" + _questId + ", step:" + _step;
        }

        boolean res = getOwner().getQuestComponent().gmSetQuestStep(ref, stepRef, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "完成当前主线任务")
    public String finishCurMainQuest()
    {
        boolean res = getOwner().getQuestComponent().gmFinishCurMainQuestStep(getContext());
        return res ? "ok" : "fail";
    }

    @ACommand(comment = "设置当前主线任务的计数[计数]")
    public String setCurMainQuestCount(int _count)
    {
        boolean res = getOwner().getQuestComponent().gmSetMainTargetCount(_count, getContext());
        return res ? "ok" : "fail";
    }

    @ACommand(comment = "快进到某一任务步骤并获得过程奖励[任务ID][任务步骤ID]")
    public String fastForwardQuestStep(long _questId, long _step)
    {
        RefQuest ref = RefQuest.getMgr().get(_questId);
        if (null == ref)
        {
            return "fail, not find quest, quest:" + _questId;
        }
        if (ref.quest_type != EQuestType.MAIN)
        {
            return "fail, only can set main quest";
        }
        RefQuestStep stepRef = RefQuestStep.getMgr().get(_step);
        if (null == stepRef)
        {
            return "fail, not find quest-step, quest:" + _questId + ", step:" + _step;
        }
        boolean res = getOwner().getQuestComponent().gmSetQuestStep(ref, stepRef, getContext());

        if (res)
        {
            for (RefQuestStep refQuestStep : RefQuestStep.getMgr().getList())
            {
                if (refQuestStep.step_id <= _step)
                {
                    getOwner().gainItemList(refQuestStep.done_gain_item_list, getContext());
                }
            }
        }

        getOwner().sendMsgToGC(getContext().getCollector().toProto());

        return res ? "ok" : "fail";
    }
}
