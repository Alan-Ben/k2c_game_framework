package NPLoginCheckServer.NPSDKLoginMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.CommonFunc;
import NPLoginCheckServer.NPLoginCheckServer;

public class LoginFailInfoLogger
{
    //开始时间
    private long _m_startRecordMs = 0;
    //总错误次数
    private long _m_totalPhpErrorCount = 0;

    //在延迟时间内是否已经发过了
    private boolean _m_hasSend = false;
    //需要预警的时间
    private int _m_warnCount = 50;
    //延迟时间
    private int _m_delayTimeSec = 30;
    //队列需要加锁避免出错
    private final MutexAtom _m_mMutex;

    public LoginFailInfoLogger()
    {
        //降低优先级避免锁冲突
        _m_mMutex = new MutexAtom();
        _m_mMutex.reducePriority(1000);
    }

    public void chgWarnThreshold(int _warnCount)
    {
        _m_warnCount = _warnCount;
    }

    public void chgDelaySec(int _delayTimeSec)
    {
        _m_delayTimeSec = _delayTimeSec;
    }

    public int getDelayPrintTimeMs()
    {
        return _m_delayTimeSec * 1000;
    }

    protected void _lock()
    {
        _m_mMutex.lock();
    }

    protected void _unlock()
    {
        _m_mMutex.unlock();
    }

    /**************
     * 设置最新需要推送的信息
     */
    public void recordFail()
    {
        _lock();
        try
        {
            long nowTimeMS = CommonFunc.getNowTimeMS();

            if (_m_startRecordMs == 0)
                _m_startRecordMs = nowTimeMS;

            _m_totalPhpErrorCount++;

            //检查是否发送失败信息
            checkSendFailInfo(nowTimeMS);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 是否需要发预警
     * @return
     */
    private boolean needSendWarn()
    {
        return _m_totalPhpErrorCount >= _m_warnCount;
    }

    private void checkSendFailInfo(long _nowTimeMs)
    {
        _lock();
        try
        {
            //距离上次发送的间隔时间
            long gapTimeMs = _nowTimeMs - _m_startRecordMs;
            //先检查之前是不是已经超过时间了
            if (gapTimeMs > getDelayPrintTimeMs())
            {
                _m_startRecordMs = CommonFunc.getNowTimeMS();
                _m_totalPhpErrorCount = 1;
                _m_hasSend = false;
            } else if (needSendWarn() && !_m_hasSend)
            {
                NPLoginCheckServer.getInstance().getDDAlert().warn("sdk login php check fail", "sdk login php check return fail " + _m_totalPhpErrorCount + " times in " + _m_delayTimeSec + " sec");

                //清空数据
                _m_hasSend = true;
            }
        } finally
        {
            _unlock();
        }
    }
}