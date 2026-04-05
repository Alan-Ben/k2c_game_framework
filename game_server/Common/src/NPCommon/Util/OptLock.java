package NPCommon.Util;

import java.util.concurrent.atomic.AtomicLong;

//异步操作锁，防止用户频繁请求
public class OptLock
{
    private AtomicLong _m_optSerial= new AtomicLong(0);
    private boolean _m_bLocked;
    private long _m_lLockTimeMs;
    private long _m_lLockSpan = 3000L;
    public OptLock(long _lockSpan)
    {
        _m_lLockSpan = _lockSpan;
    }
    public OptLock()
    {
    }
    public synchronized  long lock()
    {
        long nowMs = CommonFunc.getNowTimeMS();
        if(_m_bLocked && nowMs - _m_lLockTimeMs < _m_lLockSpan){
            return 0;
        }
        _m_bLocked =true;
        long serial = _m_optSerial.incrementAndGet();
        _m_lLockTimeMs = nowMs;
        return serial;
    }

    public boolean isLocked()
    {
        long nowMs = CommonFunc.getNowTimeMS();
        if(_m_bLocked && nowMs - _m_lLockTimeMs < _m_lLockSpan){
            return true;
        }
        return false;
    }

    public synchronized boolean unlock(long _serial)
    {
        if(_m_optSerial.get() == _serial)
        {
            _m_bLocked=false;
            _m_lLockTimeMs =0L;
            return true;
        }
        else
            return false;
    }
}
