package MGClient.ClientRequestMgr;

import ALBasicProtocolPack._IALProtocolStructure;

import java.nio.ByteBuffer;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicLong;

public class ClientRequestMgr
{
    private static class RequestNode
    {
        _IALProtocolStructure request;
        _IClientRequestHandler handler;
    }

    private static ClientRequestMgr _instance = new ClientRequestMgr();

    public static ClientRequestMgr getInstance()
    {
        return _instance;
    }

    private AtomicLong _m_serial = new AtomicLong();
    private Map<Long, RequestNode> _m_map = new ConcurrentHashMap<>();

    public long regRequest(_IALProtocolStructure _request, _IClientRequestHandler _handler)
    {
        RequestNode node = new RequestNode();
        node.handler = _handler;
        node.request = _request;

        long serial = _m_serial.incrementAndGet();
        _m_map.put(serial, node);
        return serial;
    }

    public boolean dealRequest(long _serial, int errCode, ByteBuffer _msg)
    {
        RequestNode node = _m_map.remove(_serial);
        if (node == null)
        {
            return false;
        }
        node.handler.handleResponse(errCode, _msg);
        return true;


    }
}
