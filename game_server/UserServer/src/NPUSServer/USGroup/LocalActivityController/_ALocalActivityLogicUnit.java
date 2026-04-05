package NPUSServer.USGroup.LocalActivityController;

public abstract class _ALocalActivityLogicUnit
{
    /**
     * 是否需要打断检查
     * @return
     */
    public abstract boolean needBreakAndRepeatLoopIfPass();

    /**
     * 执行逻辑
     * @return 返回是否执行成功
     */
    public abstract boolean tryExecuteLogic();

    /**
     * 执行优先级 越小越优先
     * @return
     */
    public abstract int executePriority();

    /**
     * 是否重复执行啥的
     * @return
     */
    public boolean isAlreadyExecuted()
    {
        return false;
    }
}
