package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.CommErr;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.SystemQuestComp.SystemQuestGroupInfo;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "系统任务相关命令", name = "SystemQuest")
public class CmdSystemQuest extends UsCmdBase
{
    @ACommand(comment = "增加任务计数[组id][计数值]")
    public String addCount(long _questId, long _count)
    {
        SystemQuestGroupInfo groupInfo = getOwner().getSystemQuestComponent().lookupGroup(_questId);
        if (groupInfo == null)
            return CommErr.REF_NOT_FOUND.toString();

        groupInfo.addCount(_count, getContext());
        return "ok";
    }

    @ACommand(comment = "设置任务计数[组id][计数值]")
    public String setCount(long _questId, long _count)
    {
        SystemQuestGroupInfo groupInfo = getOwner().getSystemQuestComponent().lookupGroup(_questId);
        if (groupInfo == null)
            return CommErr.REF_NOT_FOUND.toString();

        groupInfo.setCount(_count, getContext());
        return "ok";
    }

    @ACommand(comment = "修改阶段[组id][阶段]")
    public String chgStep(long _questId, int _step)
    {
        SystemQuestGroupInfo groupInfo = getOwner().getSystemQuestComponent().lookupGroup(_questId);
        if (groupInfo == null)
            return CommErr.REF_NOT_FOUND.toString();

        groupInfo.chgStep(_step, getContext());
        return "ok";
    }
}
