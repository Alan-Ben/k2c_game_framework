package NPUSServer.USGroup.LocalActivityController.LogicUnit;

import NPUSServer.USGroup.LocalActivityController._ALocalActivityLogicUnit;

/**
 * 在服务器未分组时才会处理，一旦分组则不会处理静态数据的排期
 */
public class DefaultScheduleLogicUnit extends _ALocalActivityLogicUnit
{
    @Override
    public boolean needBreakAndRepeatLoopIfPass()
    {
        //由于执行后，会导致排期变化，所以需要中断任务队列
        return true;
    }

    @Override
    public boolean tryExecuteLogic()
    {
        return false;
    }

    @Override
    public int executePriority()
    {
        return 3;
    }
}
