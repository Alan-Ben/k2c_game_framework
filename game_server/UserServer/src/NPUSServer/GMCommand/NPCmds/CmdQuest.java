package NPUSServer.GMCommand.NPCmds;

import Common.QuestEnum.EQuestType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "任务相关命令", name = "quest")
public class CmdQuest extends UsCmdBase
{
    @ACommand(comment = "设置任务（任务ID，任务步骤）")
    public String setQuest(long _questId, long _step)
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

    @ACommand(comment = "开启非限时任务")
    public String start(long _questId)
    {
        RefQuest ref = RefQuest.getMgr().get(_questId);
        boolean res = getOwner().getQuestComponent().startQuestByServer(ref, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "设置主线任务")
    public String setMainQuest(long _questId)
    {
        RefQuest ref = RefQuest.getMgr().get(_questId);
        if (null == ref)
            return "fail, not quest ref, " + _questId;

        if (ref.quest_type != EQuestType.MAIN)
            return "fail, not main quest, " + _questId;

        boolean res = getOwner().getQuestComponent().gmSetMainQuest(ref, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "完成所有主线任务")
    public String finishAllMainQuest()
    {
        getOwner().getQuestComponent().gmFinishAllMainQuest(getContext());

        return "ok";
    }

    @ACommand(comment = "设置任务目标计数（目标ID，目标计数）")
    public String setTargetCount(long _targetId, long _count)
    {
        boolean res = getOwner().getQuestComponent().gmSetTargetCount(_targetId, _count, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "重置当前主线任务步骤下的计数")
    public String resetCurMainQuestCount()
    {
        boolean res = getOwner().getQuestComponent().gmSetMainTargetCount(0, getContext());
        return res ? "ok" : "fail";
    }

    @ACommand(comment = "完成进行中主线任务当前步骤，进入到下一步骤或下一个任务")
    public String finishCurMainQuest()
    {
        boolean res = getOwner().getQuestComponent().gmFinishCurMainQuestStep(getContext());
        return res ? "ok" : "fail";
    }

    @ACommand(comment = "完成指定任务当前步骤(不在进行中会失败)，进入到下一步骤或下一个任务（任务ID）")
    public String finishQuestStep(long _questId)
    {
        boolean res = getOwner().getQuestComponent().gmFinishQuestStep(_questId, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "放弃任务（任务ID）")
    public String drop(long _questId)
    {
        getOwner().getQuestComponent().dropQuest(_questId, getContext());

        return "ok";
    }

    @ACommand(comment = "移除任务（任务ID）")
    public String remove(long _questId)
    {
        getOwner().getQuestComponent().removeQuest(_questId, getContext());

        return "ok";
    }

    @ACommand(comment = "设置主线任务完成次数，-1表示全部")
    public String setMainQuestCount(long _questId, int _count)
    {
        for (RefQuest questRef : RefQuest.getMgr().getList())
        {
            if (null == questRef)
                continue;

            if (questRef.quest_type != EQuestType.MAIN)
                continue;

            if (_questId == -1 || questRef.quest_id == _questId)
            {
                getOwner().getQuestComponent().setCount(questRef.quest_id, _count, getContext());
            }
        }

        return "ok";
    }

    @ACommand(comment = "设置任务计数（任务ID）")
    public String setCount(long _questId, int _count)
    {
        getOwner().getQuestComponent().setCount(_questId, _count, getContext());

        return "ok";
    }

    @ACommand(comment = "清理任务计数（任务ID）")
    public String clearCount(long _questId)
    {
        getOwner().getQuestComponent().setCount(_questId, 0, getContext());

        return "ok";
    }

    @ACommand(comment = "检查等待中任务")
    public String checkWaiting()
    {
        getOwner().getQuestComponent().checkAndStartAllWaitingQuests(getContext());

        return "ok";
    }

    @ACommand(comment = "重置所有任务，用完重启客户端")
    public String reset()
    {
        getOwner().getQuestComponent().gmResetAll(getContext());

        return "ok";
    }

    @ACommand(comment = "完成所有任务")
    public String setAllQuestDone()
    {
        getOwner().getQuestComponent().gmSetAllFinishDone(getContext());

        return "ok";
    }

    @ACommand(comment = "快速前进到某一任务步骤(获得过程奖励)")
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
