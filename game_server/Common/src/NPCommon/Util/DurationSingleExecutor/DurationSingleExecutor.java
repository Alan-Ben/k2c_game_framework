package NPCommon.Util.DurationSingleExecutor;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

/**
 * 重复调用，固定时间最多只执行一次
 * 如果时间内重复调用，则会在超出时间限制之后调用一次最后调用的任务
 */
public class DurationSingleExecutor
{
    //执行序列号
    private long _m_lExecutorSerialize;
    //多久时间内只允许执行一次
    private int _m_lDelayMs;
    //执行的任务对象
    private _IALSynTask _m_DealTask;
    //控制多线程访问
    private MutexAtom _m_mutex;

    public DurationSingleExecutor(int _delayMs)
    {
        _m_lExecutorSerialize = ALSerializeMaker.makeNewSerialize();
        _m_lDelayMs = _delayMs;
        _m_DealTask = null;
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /*******************
     * 尝试执行某个任务
     * @param _task
     */
    public void delayExecute(_IALSynTask _task)
    {
        if (null == _task)
            return;

        _lock();
        try
        {
            if (null == _m_DealTask)
            {
                //只有在执行任务为null，才会开始规划
                //设置新操作序列号，表示一个新的逻辑开始
                _m_lExecutorSerialize = ALSerializeMaker.makeNewSerialize();
                //此时需要设置任务并开启定时处理，因为deal为空，表示没有任务在执行
                _m_DealTask = _task;
                //此时需要重新注册一个规划任务进行处理
                ALSynTaskManager.getInstance().regTask(new DurationSingleExecutorDealTask(this, _m_lExecutorSerialize), _m_lDelayMs);
            } else
            {
                //有任务是有效的，表明有任务在规划处理，此时只需要设置ready任务即可
                //此时只需要设置等待处理任务，等待处理的时候将此任务设置为执行任务即可
                _m_DealTask = _task;
            }
        } finally
        {
            _unlock();
        }
    }

    /*
    停止本当前任务
     */
    public void stop()
    {
        //根据当前状态执行任务
        _lock();

        try
        {
            _m_lExecutorSerialize = ALSerializeMaker.makeNewSerialize();
            //重置任务
            _m_DealTask = null;
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 本任务的执行处理，用本处理来控制执行节奏
     */
    public void dealTask(long _dealSerialize)
    {
        //需要处理的任务对象
        _IALSynTask taskObj = null;

        //根据当前状态执行任务
        _lock();

        try
        {
            //匹配序列号，不一致则直接不处理
            if (_dealSerialize != _m_lExecutorSerialize)
                return;

            //获取处理任务
            taskObj = _m_DealTask;

            //设置待处理任务为空
            _m_DealTask = null;
        } finally
        {
            _unlock();
        }

        //处理任务
        if (null != taskObj)
            taskObj.run();
    }
}
