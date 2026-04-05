package NPCommon.Util.DelayExecutor;

import ALBasicServer.ALTask._IALSynTask;

/*****************
 * 延迟执行任务的实际处理任务对象
 * @author mj
 *
 */
public class DelayExecutorDealTask implements _IALSynTask
{
    //处理对象
    private DelayExecutor _m_deDelayExecutor;
    //处理序列号
    private long _m_lDealSerialize;

    public DelayExecutorDealTask(DelayExecutor _executor, long _dealSerialize)
    {
        _m_deDelayExecutor = _executor;
        _m_lDealSerialize = _dealSerialize;
    }

    @Override
    public void run()
    {
        //直接处理对象
        _m_deDelayExecutor.dealTask(_m_lDealSerialize);
    }
}
