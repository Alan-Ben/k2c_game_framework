package NPCommon.Util.DurationSingleTask;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

/********
 * 尝试处理某个事情的时候，根据实际的处理情况保证在一定时间内只能处理一次
 * @author mj
 *
 */
public abstract class _ADurationSingleDealer
{
    //保证多久时间内只能执行一次
    private long _m_lDurationMs;
    //最后一次允许执行操作的时间戳
    private long _m_lLastCanDealTimeMs;

    //处理操作序列号
    private long _m_lDealSerialize;
    //当前是否有任务在执行
    private boolean _m_bIsTaskSchedule;

    //保证判断的原子互斥锁
    private MutexAtom _m_mutex;

    public _ADurationSingleDealer()
    {
        _m_lDurationMs = 1000;
        _m_lLastCanDealTimeMs = 0;

        _m_lDealSerialize = 0;
        _m_bIsTaskSchedule = false;

        _m_mutex = new MutexAtom();
    }

    public _ADurationSingleDealer(long _delayMs)
    {
        _m_lDurationMs = _delayMs;
        _m_lLastCanDealTimeMs = 0;

        _m_lDealSerialize = 0;
        _m_bIsTaskSchedule = false;

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

    /**********************
     * 尝试处理本操作
     */
    public void tryDeal()
    {
        _lock();

        try
        {
            long nowTimeMS = ALBasicCommonFun.getNowTimeMS();

            //判断当前事件是否超出可执行时间
            //同时需要判断是否在规划中，因为在临界时间点是可能出现超过规定时间，但是还有规划任务未执行的情况
            if (_m_lLastCanDealTimeMs < nowTimeMS && !_m_bIsTaskSchedule)
            {
                //直接执行
                _delayDealTask(0);
                return;
            }

            //此时时间在无法执行的状态
            //判断是否已经在执行，决定是否需要注册任务
            if (_m_bIsTaskSchedule)
            {
                //已经在计划中不处理后续的规划
                return;
            }

            //注册到指定时间之后执行
            _delayDealTask(_m_lDurationMs);
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 实际的处理函数
     */
    protected void _realDeal(long _dealSerialize)
    {
        if (_dealSerialize != _m_lDealSerialize)
            return;

        //此处优先设置为未处理是为了避免如果在处理之后设置，可能在两个处理的临界时间点出现某个处理被遗漏的情况
        _lock();

        try
        {
            //重置当前的处理状态
            _m_bIsTaskSchedule = false;
        } finally
        {
            _unlock();
        }

        //处理实际的函数
        _deal();
    }

    /**************
     * 直接处理的函数，调用此函数会直接在指定的时间设置处理
     */
    protected void _delayDealTask(long _delayTimeMS)
    {
        //设置下一次允许执行的时间戳
        _m_lLastCanDealTimeMs = ALBasicCommonFun.getNowTimeMS() + _m_lDurationMs;
        //累加序列号
        _m_lDealSerialize++;
        //设置在处理
        _m_bIsTaskSchedule = true;

        //注册任务处理
        ALSynTaskManager.getInstance().regTask(new SynDurationSingleDealerDealTask(this, _m_lDealSerialize), _delayTimeMS);
    }

    /********************
     * 实际子类需要继承的处理函数
     */
    protected abstract void _deal();


    /****************
     * 定时处理操作的处理任务
     * @author mj
     *
     */
    public class SynDurationSingleDealerDealTask implements _IALSynTask
    {
        //处理对象
        private _ADurationSingleDealer _m_sdDealer;
        //处理操作的序列号
        private long _m_lDealSerialize;

        public SynDurationSingleDealerDealTask(_ADurationSingleDealer _dealer, long _dealSerialize)
        {
            _m_sdDealer = _dealer;
            _m_lDealSerialize = _dealSerialize;
        }

        @Override
        public void run()
        {
            if (null == _m_sdDealer)
                return;

            //尝试处理对应类的处理函数
            _m_sdDealer._realDeal(_m_lDealSerialize);
        }

    }
}
