package NPPlatServer.PlatListenerMgr;

import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class PlatListenerMgr
{
    private static PlatListenerMgr _instance = new PlatListenerMgr();

    public static PlatListenerMgr getInstance()
    {
        return _instance;
    }

    private static long makeKey(int _serverType, int _serverTypeId)
    {
        return _serverTypeId * 1000 + _serverType;
    }

    private Map<Long, _AWCGBasicServerListener> _m_mListenerMap = new ConcurrentHashMap<>();

    public void register(int _serverType, int _serverTypeId, _AWCGBasicServerListener _listener)
    {
        _m_mListenerMap.put(makeKey(_serverType, _serverTypeId), _listener);
    }

    public void unRegister(int _serverType, int _serverTypeId)
    {
        _m_mListenerMap.remove(makeKey(_serverType, _serverTypeId));
    }

    public _AWCGBasicServerListener lookupListener(int _serverType, int _serverTypeId)
    {
        return _m_mListenerMap.get(makeKey(_serverType, _serverTypeId));
    }

    public List<_AWCGBasicServerListener> getListenerListOfType(int serverType)
    {
        List<_AWCGBasicServerListener> retList = new ArrayList<_AWCGBasicServerListener>();
        for (_AWCGBasicServerListener listener : _m_mListenerMap.values())
        {
            if (listener.getServerType() == serverType)
            {
                retList.add(listener);
            }
        }
        return retList;
    }

    public ArrayList<_AWCGBasicServerListener> getAllListenerList()
    {
        return new ArrayList<>(_m_mListenerMap.values());
    }
}
