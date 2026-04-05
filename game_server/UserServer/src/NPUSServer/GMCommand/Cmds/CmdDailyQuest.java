package NPUSServer.GMCommand.Cmds;

import Common.QuestEnum.EDailyQuestType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestFreshGroup;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestInfo;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "日常任务相关命令", name = "dailyQuest")
public class CmdDailyQuest extends UsCmdBase
{
    @ACommand(comment = "刷新任务(DAY or WEEK)")
    public String refresh(EDailyQuestType type)
    {
        DailyQuestFreshGroup group = getOwner().getDailyQuestComponent().lookupGroup(type);
        if (group == null)
        {
            return "not found";
        }
        long nowTimeMS = CommonFunc.getNowTimeMS();

        group.setFreshTime(-1);
        group.tryFresh(nowTimeMS, group.getSerial());
        return "ok";
    }

    @ACommand(comment = "设置任务计数[任务id][计数值]")
    public String setCount(long _questId, long _count)
    {
        DailyQuestInfo questInfo = getOwner().getDailyQuestComponent().lookupQuestById(_questId);
        if (questInfo == null)
        {
            return "not found";
        }
        questInfo.setCount(_count);

        return "ok";
    }

    @ACommand(comment = "设置下一次刷新时间，（秒）")
    public String setFreshTime(int _sec)
    {
        getOwner().getDailyQuestComponent().cmdSetFreshTime(CommonFunc.getNowTimeMS()
                + _sec * 1000L);

        return "ok";
    }

    @ACommand(comment = "清除数据")
    public String clear()
    {
        getOwner().getDailyQuestComponent().cmdClear();

        return "ok";
    }
}
