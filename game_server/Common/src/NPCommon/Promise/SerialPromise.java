package NPCommon.Promise;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALBasicMutex.MutexObject;

import java.util.LinkedList;

/******
 * 按顺序执行的Promise
 */
public class SerialPromise
{
    private volatile Then _m_over; //最后执行的回调
    private final MutexAtom _m_locker = new MutexAtom();
    private final LinkedList<Then> _m_taskList = new LinkedList<>();//执行任务列表

    private boolean _m_bBroken = false; //是否执行过程中中断了
    private MutexObject _m_execLocker; //外部回调锁

    public SerialPromise(MutexObject _locker)
    {
        _m_execLocker = _locker;
    }

    @FunctionalInterface
    public interface Then
    {
        void action(SerialPromise _promise);
    }

    /******
     * 定义一个任务，如果是第一个任务，则启动任务链
     * @param _then
     * @return
     */
    public SerialPromise then(Then _then)
    {
        Then startThen = null;
        _m_locker.lock();
        try
        {
            if (_m_bBroken)
                return this;
            if (_m_taskList.isEmpty())
            {
                startThen = _then;
            }
            _m_taskList.addLast(_then);
        } finally
        {
            _m_locker.unlock();
        }

        if (null != startThen)
        {
            execute(startThen);
        }
        return this;
    }

    /*******
     * 完成当前任务，有可能执行下一个任务或调用Over
     */
    public void commit()
    {
        Then over = null;
        Then nextThen = null;
        _m_locker.lock();
        try
        {
            _m_taskList.pollFirst();
            if (!_m_taskList.isEmpty())
            {
                nextThen = _m_taskList.peekFirst();
            } else
            {
                if (_m_over != null)
                {
                    over = _m_over;
                    _m_over = null;
                }
            }
        } finally
        {
            _m_locker.unlock();
        }
        if (nextThen != null)
        {
            execute(nextThen);
            return;
        }

        if (null != over)
        {
            execute(over);
        }
    }

    /******
     * 清空任务链，直接跳到Over执行，后面不会加入新的任务。
     */
    public void breakOut()
    {
        Then over = null;
        _m_locker.lock();
        try
        {
            _m_bBroken = true;
            _m_taskList.clear();
            if (_m_over != null)
            {
                over = _m_over;
                _m_over = null;
            }
        } finally
        {
            _m_locker.unlock();
        }

        if (null != over)
        {
            execute(over);
        }

    }

    public void over(Then _then)
    {
        Then over = null;
        _m_locker.lock();
        try
        {
            _m_over = _then;
            if (_m_taskList.isEmpty())
            {
                over = _m_over;
                _m_over = null;
            }
        } finally
        {
            _m_locker.unlock();
        }
        if (null != over)
        {
            execute(over);
        }
    }

    private void execute(Then _then)
    {
        if (_m_execLocker != null)
        {
            _m_execLocker.lock();
            try
            {
                _then.action(this);
            } finally
            {
                _m_execLocker.unlock();
            }
        } else
            _then.action(this);
    }
}
