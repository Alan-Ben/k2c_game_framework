package NPCommon.Util.DurationSingleExecutor;

import ALBasicServer.ALTask._IALSynTask;

/*****************
 * 延迟执行任务的实际处理任务对象
 * @author mj
 *
 */
public class DurationSingleExecutorDealTask implements _IALSynTask
{
    //处理对象
    private DurationSingleExecutor _m_deDurationSingleExecutor;
    //处理序列号
    private long _m_lDealSerialize;

    public DurationSingleExecutorDealTask(DurationSingleExecutor _executor, long _dealSerialize)
    {
        _m_deDurationSingleExecutor = _executor;
        _m_lDealSerialize = _dealSerialize;
    }

    @Override
    public void run()
    {
        //直接处理对象
        _m_deDurationSingleExecutor.dealTask(_m_lDealSerialize);
    }
}
