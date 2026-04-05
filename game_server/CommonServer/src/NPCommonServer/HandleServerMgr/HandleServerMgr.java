package NPCommonServer.HandleServerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import CSDB.Bo.HandleServerBO;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommonServer;

import java.util.ArrayList;

public class HandleServerMgr
{
    //服务器类型
    private int _m_iServerType;

    //服务器权重上限，0-表示不设置限制
    private int _m_iHandleUserWeightLimit;

    //所有CrossGame服务器负载信息列表
    private ArrayList<HandleServerInfo> _m_alServerList;

    //锁对象
    private MutexAtom _m_mutex;

    public HandleServerMgr(int _serverType, int _handleUserWeightLimit)
    {
        _m_iServerType = _serverType;

        _m_iHandleUserWeightLimit = _handleUserWeightLimit;

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

    public int getServerType() {return _m_iServerType;}

    public void setHandleUserWeightLimit(int _value) {_m_iHandleUserWeightLimit = _value;}
    public int getHandleUserWeightLimit() {return _m_iHandleUserWeightLimit;}

    protected void _initServer(HandleServerInfo _info)
    {
        _m_alServerList.add(_info);
    }

    /**
     * 查找指定服务器信息
     * @param _serverTypeId
     * @return
     */
    public HandleServerInfo lookupServerById(int _serverTypeId)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alServerList.size(); i++)
            {
                HandleServerInfo server = _m_alServerList.get(i);
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
    public void regHandleServer(int _serverTypeId)
    {
        _lock();

        try
        {
            HandleServerInfo server = lookupServerById(_serverTypeId);
            if (server != null)
            {
                server.setEnable(true);
                CommLog.error("HandleServerInfoMgr.regHandleServer duplicate reg server:{}-{}.", _m_iServerType, _serverTypeId);
            }
            else
            {
                HandleServerBO bo = new HandleServerBO();
                bo.setServerType(NPCommonServer.getInstance().getBM(), _m_iServerType);
                bo.setTypeId(NPCommonServer.getInstance().getBM(), _serverTypeId);
                bo.setWeight(NPCommonServer.getInstance().getBM(), 0);
                bo.insert(NPCommonServer.getInstance().getBM());

                HandleServerInfo newServer = new HandleServerInfo(this, bo);
                newServer.setEnable(true);

                _m_alServerList.add(newServer);
                CommLog.info("HandleServerInfoMgr.regHandleServer server:{}-{} limit:{} suc.", _m_iServerType, _serverTypeId, _m_iHandleUserWeightLimit);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 注销
     * @param _serverTypeId
     */
    public void unRegHandleServer(int _serverTypeId)
    {
        _lock();

        try
        {
            //将对象从队列中删除
            for (int i = 0; i < _m_alServerList.size(); i++)
            {
                HandleServerInfo server = _m_alServerList.get(i);
                if (null == server)
                    continue;

                if (server.getServerTypeId() == _serverTypeId)
                {
                    server.setEnable(false);
                    CommLog.info("HandleServerInfoMgr.unRegHandleServer server:{}-{} suc.", _m_iServerType, _serverTypeId);
                    return;
                }
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 尝试获取负载最低的服务器
     * @param _tmpAddWeight
     * @return
     */
    public HandleServerInfo tryHandleServer(int _tmpAddWeight)
    {
        HandleServerInfo selectServer = null;

        _lock();

        try
        {
            HandleServerInfo tmpServer = null;
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
        }
        finally
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
