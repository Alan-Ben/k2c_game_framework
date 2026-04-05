package NPGateServer.NPGSGCMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPGateServer.NPGCListener.NPGSGCListener;

import java.util.HashMap;

/************
 * 客户端连接对象管理器
 * @author Administrator
 *
 */
public class NPGSGCMgr
{
    private static NPGSGCMgr _g_instance = new NPGSGCMgr();

    public static NPGSGCMgr getInstance()
    {
        return _g_instance;
    }

    private HashMap<Long, NPGSGCListener> _m_hmGCListenerMap;

    private MutexAtom _m_mutex;

    protected NPGSGCMgr()
    {
        _m_hmGCListenerMap = new HashMap<Long, NPGSGCListener>();
        _m_mutex = new MutexAtom();
    }

    /******************
     * 注册GC连接对象
     * @param _listener
     */
    public NPGSGCListener regGCListener(NPGSGCListener _listener)
    {
        if (null == _listener)
            return null;

        _lock();

        try
        {
            return _m_hmGCListenerMap.put(_listener.getSessionId(), _listener);
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 注销GC连接对象
     * @param _listener
     */
    public void unregGCListener(NPGSGCListener _listener)
    {
        if (null == _listener)
            return;

        _lock();

        try
        {
            _m_hmGCListenerMap.remove(_listener.getSessionId());
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 查询GC连接对象
     * @param _listener
     */
    public NPGSGCListener getGCListener(long _sessionId)
    {
        _lock();

        try
        {
            return _m_hmGCListenerMap.get(_sessionId);
        } finally
        {
            _unlock();
        }
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }
}
