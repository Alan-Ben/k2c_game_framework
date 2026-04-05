package NPPlatServer.NPPlatServerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPPlatServer.NPPS_Listener.PS_CSListener;
import NPPlatServer.NPPS_Listener.PS_GLSListener;
import NPServerProtocolWriter.NP2CS.Msg.NP2CS_Writer_001_BasicOp;

import java.util.ArrayList;

public class NPPlatGLSMgr
{
    private static NPPlatGLSMgr _g_instance = new NPPlatGLSMgr();
    public static NPPlatGLSMgr getInstance()
    {
        return _g_instance;
    }

    private ArrayList<PS_GLSListener> _m_alGLSListenerList;
    private MutexAtom _m_mutex;

    public NPPlatGLSMgr()
    {
        _m_alGLSListenerList = new ArrayList<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }
    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 构造数据列表
     * @param _typeIdList
     */
    public void makeProtoList(ArrayList<Integer> _typeIdList)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alGLSListenerList.size(); i++)
            {
                PS_GLSListener listener = _m_alGLSListenerList.get(i);
                if (null == listener)
                    continue;

                _typeIdList.add(listener.getServerTypeId());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册
     * @param _listener
     */
    public void reg(PS_GLSListener _listener)
    {
        if (null == _listener)
            return;

        _lock();

        try
        {
            // 将对象加入队列
            _m_alGLSListenerList.add(_listener);

            // 发送消息通知common服务器，添加房间服务器
            PS_CSListener.sendCustMsg(NP2CS_Writer_001_BasicOp.make_010_AddHandleServer(_listener.getServerType(), _listener.getServerTypeId()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销
     * @param _listener
     */
    public void unReg(PS_GLSListener _listener)
    {
        if (null == _listener)
            return;

        _lock();

        try
        {
            if (_m_alGLSListenerList.remove(_listener))
            {
                // 发送消息通知common服务器，添加房间服务器
                PS_CSListener.sendCustMsg(NP2CS_Writer_001_BasicOp.make_011_RemoveHandleServer(_listener.getServerType(), _listener.getServerTypeId()));
            }
        } finally
        {
            _unlock();
        }
    }
}
