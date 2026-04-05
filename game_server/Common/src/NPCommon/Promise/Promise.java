package NPCommon.Promise;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.StringFunc;

import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;

/****
 * 并发执行的Promise
 */
public class Promise
{
    private final BitSet _m_flag = new BitSet(5);
    private volatile Then _m_over;
    private final MutexAtom _m_locker = new MutexAtom();
    private boolean _m_bHasErr = false;
    private final List<String> _m_errorList = new ArrayList<>();

    @FunctionalInterface
    public interface Then
    {
        void action(Promise _promise);
    }

    public Promise then(int index, Then _then)
    {
        _m_locker.lock();
        try
        {
            _m_flag.set(index);
        } finally
        {
            _m_locker.unlock();
        }

        _then.action(this);
        return this;
    }

    public void commit(int index)
    {
        _m_locker.lock();
        Then over = null;
        try
        {
            _m_flag.clear(index);

            if (_m_flag.isEmpty() && _m_over != null)
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
            over.action(this);
        }
    }

    public void over(Then _then)
    {
        Then over = null;
        _m_locker.lock();
        try
        {
            _m_over = _then;
            if (_m_flag.isEmpty())
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
            over.action(this);
        }

    }

    public void err(String _msg)
    {
        _m_bHasErr = true;
        _m_errorList.add(_msg);

    }

    public boolean isSucc()
    {
        return !_m_bHasErr;
    }

    public String getErrMsgs()
    {
        return StringFunc.joinString("\n", _m_errorList);
    }

}
