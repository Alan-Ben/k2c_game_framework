package NPGateServer.USServerInfoListMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

/**
 * @description: US服务器信息列表管理器
 * @author: ricci
 * @date: 2022-06-25 10:24:46
 */
public class USServerIndexInfoListMgr
{
    /**
     * 服务器 logicId--服务器索引信息 的映射
     */
    private final Map<Integer, NpServerObj_SYS_ServerIndexInfo> _m_serverIndexInfoMap;
    //////单例的//////
    private static final USServerIndexInfoListMgr _s_instance = new USServerIndexInfoListMgr();

    public static USServerIndexInfoListMgr getInstance()
    {
        return _s_instance;
    }

    private MutexAtom _m_mutex;
    /**
     * 初始化任务是否开启，保证初始化任务同时只有一个运行
     */
    private volatile boolean _m_taskIsOpen;

    /**
     * 是否已经初始化
     */
    private volatile boolean _m_isInited;

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    private USServerIndexInfoListMgr()
    {
        this._m_serverIndexInfoMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    //region get&&set
    public boolean isInited()
    {
        return _m_isInited;
    }

    private void setInited(boolean _init)
    {
        _m_isInited = _init;
    }

    public boolean isTaskIsOpen()
    {
        return _m_taskIsOpen;
    }

    public void setTaskIsOpen(boolean _taskIsOpen)
    {
        this._m_taskIsOpen = _taskIsOpen;
    }


    /**
     * 所有逻辑id列表
     * @return Collection<Integer>
     */
    public Collection<Integer> getAllServerLogicIdList()
    {
        _lock();
        try
        {
            return _m_serverIndexInfoMap.keySet();
        } finally
        {
            _unlock();
        }
    }
    //endregion

    /**
     * 初始化
     */
    public void init()
    {
        if (_m_taskIsOpen)
        {
            return;
        }
        _m_taskIsOpen = true;
        USServerInfoInitRequestTask task = new USServerInfoInitRequestTask();
        //启动任务，每3秒去CS上取一次服务器信息列表，直到取到为止
        ALSynTaskManager.getInstance().regTask(task, 1000);
    }

    /**
     * 初始化服务器列表
     * @param _serverIndexList 服务器索引信息
     */
    public void initLoadServerList(ArrayList<NpServerObj_SYS_ServerIndexInfo> _serverIndexList)
    {
        _lock();
        try
        {
            _m_serverIndexInfoMap.clear();

            for (NpServerObj_SYS_ServerIndexInfo indexInfo : _serverIndexList)
            {
                if (indexInfo == null)
                {
                    continue;
                }
                _m_serverIndexInfoMap.put(indexInfo.getServerLogicId(), indexInfo);
            }
            setInited(true);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新服务器索引信息
     * @param _serverIndexList US服务器索引信息
     */
    public void updateServerItem(ArrayList<NpServerObj_SYS_ServerIndexInfo> _serverIndexList)
    {
        _lock();
        try
        {
            for (NpServerObj_SYS_ServerIndexInfo indexInfo : _serverIndexList)
            {
                _m_serverIndexInfoMap.put(indexInfo.getServerLogicId(), indexInfo);
            }
        } finally
        {
            _unlock();
        }
    }

    public NpServerObj_SYS_ServerIndexInfo lookupByLogicId(int _serverLogicId)
    {
        _lock();
        try
        {
            return _m_serverIndexInfoMap.get(_serverLogicId);
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (NpServerObj_SYS_ServerIndexInfo info : _m_serverIndexInfoMap.values())
        {
            stringBuilder.append("\n[").append(info.getServerLogicId()).append(":").append(info.getServerTypeId()).append("\n");
        }
        return "USServerIndexInfoListMgr{" +
                "_m_serverIndexInfoMap=" + stringBuilder +
                ", _m_taskIsOpen=" + _m_taskIsOpen +
                ", _m_isInited=" + _m_isInited +
                '}';
    }
}
