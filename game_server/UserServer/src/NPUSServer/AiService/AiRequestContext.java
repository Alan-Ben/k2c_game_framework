package NPUSServer.AiService;

import NPCommon.Util.CallBack._ICallBackIntT;

public class AiRequestContext
{
    private final long _m_requestId;
    private final long _m_cid;
    private final long _m_createTimeMs;
    private final _ICallBackIntT<String> _m_callback;

    public AiRequestContext(long _requestId, long _cid, long _createTimeMs, _ICallBackIntT<String> _callback)
    {
        _m_requestId = _requestId;
        _m_cid = _cid;
        _m_createTimeMs = _createTimeMs;
        _m_callback = _callback;
    }

    public long getCid()
    {
        return _m_cid;
    }

    public long getCreateTimeMs()
    {
        return _m_createTimeMs;
    }

    public _ICallBackIntT<String> getCallback()
    {
        return _m_callback;
    }
}
