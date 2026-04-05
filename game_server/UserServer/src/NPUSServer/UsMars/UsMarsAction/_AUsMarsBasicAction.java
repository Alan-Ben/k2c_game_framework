package NPUSServer.UsMars.UsMarsAction;

/**
 * 火星行为的接口对象
 * 用于在总管理器中进行处理
 */
public abstract class _AUsMarsBasicAction
{
    /**
     * 获取本行为触发的时间点，仅会在触发的时候进行处理。过程不会做tick
     */
    public abstract long getTriggerTimeMS();

    /**
     * 触发结果行为的处理，此行为仅会触发一次
     * @param _nowTimeMs
     */
    protected abstract void _trigger(long _nowTimeMs);

    /**
     * 处理取消操作的行为
     */
    protected abstract void _dealCancel();

    /**
     * 清理行为数据
     */
    protected abstract  void _clearData();
}
