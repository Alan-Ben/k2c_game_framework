package NPPlatServer.NPPlatServerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import Common.Common_USInfo;
import NPPlatServer.NPPS_Listener.PS_USListener;

import java.util.ArrayList;

/****************
 * 平台中管理的对应区域信息对象
 *
 * @author Administrator
 *
 */
public class NPPlatUSMgr
{
    private static NPPlatUSMgr _g_instance = new NPPlatUSMgr();

    public static NPPlatUSMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = null;

        return _g_instance;
    }

    /**
     * 本区域的用户服务器列表
     */
    private ArrayList<PS_USListener> _m_lAreaUSList;

    /**
     * 管理类操作锁，由于管理类中的列表操作不频繁，因此与统计共用一个锁
     */
    private MutexAtom _m_mutex;

    protected NPPlatUSMgr()
    {
        _m_lAreaUSList = new ArrayList<PS_USListener>();

        _m_mutex = new MutexAtom();
    }

    /*****************
     * 注册用户服务器对象
     *
     * @param _gsListener
     */
    public void regUS(PS_USListener _usListener)
    {
        if (null == _usListener)
            return;

        _lock();

        try
        {
            // 将对象加入队列
            _m_lAreaUSList.add(_usListener);

            ALServerLog.Sys("Reg User Server: " + _usListener.getServerTypeId());
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 注销用户服务器对象
     *
     * @param _gsListener
     */
    public void unregUS(PS_USListener _usListener)
    {
        if (null == _usListener)
            return;

        _lock();

        try
        {
            // 将对象从队列中删除
            for (int i = 0; i < _m_lAreaUSList.size(); i++)
            {
                if (_m_lAreaUSList.get(i) == _usListener)
                {
                    // 从队列移除
                    _m_lAreaUSList.remove(i);

                    return;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 获取所有的用户服务器列表
     * @param _uecList
     */
    public void getAllUSInfo(ArrayList<Common_USInfo> _uecList)
    {
        if (null == _uecList)
            return;

        _lock();

        try
        {
            PS_USListener tmpListener = null;
            for (int i = 0; i < _m_lAreaUSList.size(); i++)
            {
                tmpListener = _m_lAreaUSList.get(i);
                if (null == tmpListener)
                    continue;

                Common_USInfo usInfo = new Common_USInfo();
                usInfo.setServerTypeId(tmpListener.getServerTypeId());
                usInfo.setUsServerId(tmpListener.getServerId());
                _uecList.add(usInfo);
            }
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
