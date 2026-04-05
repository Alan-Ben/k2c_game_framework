package NPCommonServer.NPCrossGameServerHandleMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;

import java.util.ArrayList;

public class NPCrossGameServerMgr
{
    private static NPCrossGameServerMgr _g_instance = new NPCrossGameServerMgr();

    public static NPCrossGameServerMgr getInstance()
    {
        return _g_instance;
    }

    //所有CrossGame服务器负载信息列表
    private ArrayList<NPCrossGameServerInfo> _m_alServerList;
    //锁对象
    private MutexAtom _m_mutex;

    public NPCrossGameServerMgr()
    {
        _m_alServerList = new ArrayList<>();
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
     * 查找指定服务器信息
     * @param _serverTypeId
     * @return
     */
    public NPCrossGameServerInfo lookupServerById(int _serverTypeId)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alServerList.size(); i++)
            {
                NPCrossGameServerInfo server = _m_alServerList.get(i);
                if (null == server)
                    continue;

                if (server.getServerTypeId() == _serverTypeId)
                    return server;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册
     * @param _serverTypeId
     */
    public void regCrossGameServer(int _serverTypeId)
    {
        _lock();

        try
        {
            NPCrossGameServerInfo server = lookupServerById(_serverTypeId);
            if (server != null)
            {
                ALServerLog.Error("NPCrossGameServerMgr.regCrossGameServer duplicate cross-game server registration:" + _serverTypeId + " !");
                return;
            }

            _m_alServerList.add(new NPCrossGameServerInfo(_serverTypeId));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销
     * @param _serverTypeId
     */
    public void unregCrossGameServer(int _serverTypeId)
    {
        _lock();

        try
        {
            //将对象从队列中删除
            for (int i = 0; i < _m_alServerList.size(); i++)
            {
                NPCrossGameServerInfo server = _m_alServerList.get(i);
                if (null == server)
                    continue;

                if (server.getServerTypeId() == _serverTypeId)
                {
                    _m_alServerList.remove(i);

                    return;
                }
            }
        } finally
        {
            _unlock();
        }

        ALServerLog.Error("NPCrossGameServerMgr.unregRS can not find Cross-Game for id:" + _serverTypeId + " !");
    }

    /**
     * 尝试获取合适的Cross-Game服务器
     * @param _tmpAddWeight
     * @return
     */
    public NPCrossGameServerInfo tryHandleCrossGameServer(int _tmpAddWeight)
    {
        NPCrossGameServerInfo selectServer = null;

        _lock();

        try
        {
            NPCrossGameServerInfo tmpServer = null;
            long minWeight = Long.MAX_VALUE;

            // 从队列中寻找一个处理权重最低且还可处理用户的服务器
            for (int i = 0; i < _m_alServerList.size(); i++)
            {
                tmpServer = _m_alServerList.get(i);
                if (null == tmpServer || !tmpServer.canHandle())
                    continue;

                // 判断是否最小权重
                if (null == selectServer || minWeight > tmpServer.getHandleUserWeight())
                {
                    // 设置选中本服务器
                    selectServer = tmpServer;

                    minWeight = tmpServer.getHandleUserWeight();
                }
            }
        } finally
        {
            _unlock();
        }

        // 尝试由选中服务器处理用户
        if (null == selectServer || !selectServer.addHandleUser(_tmpAddWeight))
            return null;

        // 返回对应服务器
        return selectServer;
    }
}
