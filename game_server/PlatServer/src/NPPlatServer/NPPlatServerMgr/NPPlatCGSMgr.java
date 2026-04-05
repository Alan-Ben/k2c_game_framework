package NPPlatServer.NPPlatServerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPPlatServer.NPPS_Listener.PS_CGSListener;
import NPPlatServer.NPPS_Listener.PS_CSListener;
import NPServerProtocolWriter.NP2CS.Msg.NP2CS_Writer_001_BasicOp;

import java.util.ArrayList;

public class NPPlatCGSMgr
{
    private static NPPlatCGSMgr _g_instance = new NPPlatCGSMgr();

    public static NPPlatCGSMgr getInstance()
    {
        return _g_instance;
    }

    private ArrayList<PS_CGSListener> _m_alCGSListenerList;
    private MutexAtom _m_mutex;

    public NPPlatCGSMgr()
    {
        _m_alCGSListenerList = new ArrayList<>();

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
            for (int i = 0; i < _m_alCGSListenerList.size(); i++)
            {
                PS_CGSListener listener = _m_alCGSListenerList.get(i);
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
     * @param _cgsListener
     */
    public void regCGS(PS_CGSListener _cgsListener)
    {
        if (null == _cgsListener)
            return;

        _lock();

        try
        {
            // 将对象加入队列
            _m_alCGSListenerList.add(_cgsListener);

            // 发送消息通知common服务器，添加房间服务器
            PS_CSListener.sendCustMsg(NP2CS_Writer_001_BasicOp.make_006_AddCrossGameServer(_cgsListener.getServerTypeId()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销
     * @param _cgsListener
     */
    public void unregCGS(PS_CGSListener _cgsListener)
    {
        if (null == _cgsListener)
            return;

        _lock();

        try
        {
            if (_m_alCGSListenerList.remove(_cgsListener))
            {
                // 发送消息通知common服务器，添加房间服务器
                PS_CSListener.sendCustMsg(NP2CS_Writer_001_BasicOp.make_007_RomoveCrossGameServer(_cgsListener.getServerTypeId()));
            }
        } finally
        {
            _unlock();
        }
    }
}
