package NPCommon.Util.DelayExecutor;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Util.CommonFunc;

/********
 * 重复调用，固定时间最多只执行一次
 * 如果时间内重复调用，则会在超出时间限制之后调用一次最后调用的任务
 * @author scott
 *
 */
public class DelayExecutor
{
    //执行序列号
    private long _m_lExecutorSerialize;

    //多久时间内只允许执行一次
    private long _m_lDelayMs = 1000;
    //最后一次执行的时间
    private long _m_lTagetUpdateTimeMs;

    //执行的任务对象
    private _IALSynTask _m_DealTask;

    public DelayExecutor(long _delayMs)
    {
        _m_lExecutorSerialize = ALSerializeMaker.makeNewSerialize();

        _m_lDelayMs = _delayMs;
        _m_lTagetUpdateTimeMs = 0;
        _m_DealTask = null;
    }

    /*******************
     * 尝试执行某个任务
     * @param _task
     */
    public synchronized void delayExecute(_IALSynTask _task)
    {
        if (null == _task)
            return;

        long nowMs = CommonFunc.getNowTimeMS();

        //计算时间差
        long elapsedSpan = nowMs - _m_lTagetUpdateTimeMs;
        if (null == _m_DealTask)
        {
            //只有在执行任务为null，才会开始规划
            //设置新操作序列号，表示一个新的逻辑开始
            _m_lExecutorSerialize = ALSerializeMaker.makeNewSerialize();
            //此时需要设置任务并开启定时处理，因为deal为空，表示没有任务在执行
            _m_DealTask = _task;
            //此时需要重新注册一个规划任务进行处理,如果时间低于间隔，则会注册延迟任务，如果高于间隔则直接会马上处理。所以这里不做判断
            ALSynTaskManager.getInstance().regTask(new DelayExecutorDealTask(this, _m_lExecutorSerialize), _m_lDelayMs - elapsedSpan);
        } else
        {
            //有任务是有效的，表明有任务在规划处理，此时只需要设置ready任务即可
            //此时只需要设置等待处理任务，等待处理的时候将此任务设置为执行任务即可
            _m_DealTask = _task;
        }
    }

    /*
    停止本当前任务
     */
    public synchronized void stop()
    {
        //根据当前状态执行任务
        _m_lExecutorSerialize = ALSerializeMaker.makeNewSerialize();
        //重置任务
        _m_DealTask = null;
    }

    /***************
     * 本任务的执行处理，用本处理来控制执行节奏
     * 只在状态读写时持锁，任务执行在锁外进行，避免长时间持锁阻塞其他调用
     */
    public void dealTask(long _dealSerialize)
    {
        _IALSynTask taskObj;

        //只在读写状态时加锁
        synchronized (this)
        {
            //匹配序列号，不一致则直接不处理
            if (_dealSerialize != _m_lExecutorSerialize)
                return;

            //获取处理任务
            taskObj = _m_DealTask;
            //设置最后处理时间，保证处理的时间间隔
            _m_lTagetUpdateTimeMs = CommonFunc.getNowTimeMS();
            //设置待处理任务为空
            _m_DealTask = null;
        }

        //锁外执行任务，避免 run() 耗时阻塞 delayExecute/stop
        if (null != taskObj)
            taskObj.run();
    }
}
