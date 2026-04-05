package NPCommon.DB;

import ALBasicServer.ALTask._IALAsynCallBackTask;

public abstract class _AUpdateCallback<T> implements _IALAsynCallBackTask<T>
{
    //存储本操作是否有错误
    private boolean _m_bHasErr;

    public boolean getHasErr()
    {
        return _m_bHasErr;
    }

    public void markErr()
    {
        _m_bHasErr = true;
    }
}