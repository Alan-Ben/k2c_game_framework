package NPHttpServer.HttpAiService;

public class HttpAiRequestContext
{
    private int _m_usId;
    private long _m_usRequestId;
    private long _m_createTimeMs;

    public HttpAiRequestContext(int _usId, long _usRequestId, long _createTimeMs)
    {
        _m_usId = _usId;
        _m_usRequestId = _usRequestId;
        _m_createTimeMs = _createTimeMs;
    }

    public int getUsId()
    {
        return _m_usId;
    }

    public long getUsRequestId()
    {
        return _m_usRequestId;
    }

    public long getCreateTimeMs()
    {
        return _m_createTimeMs;
    }
}
