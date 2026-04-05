package NPCommonServer.CrossRankServerHandleMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2CRS.Request.NP2CRS_R_Writer_001_BasicOp;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;

public class CrossRankServerMgr
{
    private static CrossRankServerMgr _g_instance = new CrossRankServerMgr();

    public static CrossRankServerMgr getInstance()
    {
        return _g_instance;
    }

    //所有CrossGame服务器负载信息列表
    private ArrayList<CrossRankServerInfo> _m_alServerList;
    //锁对象
    private MutexAtom _m_mutex;

    public CrossRankServerMgr()
    {
        _m_alServerList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 查找指定服务器信息
     * @param _serverTypeId
     * @return
     */
    public CrossRankServerInfo lookupServerById(int _serverTypeId)
    {
        _lock();
        try
        {
            for (CrossRankServerInfo server : _m_alServerList)
            {
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
    public void regServer(int _serverTypeId)
    {
        _lock();
        try
        {
            CrossRankServerInfo server = lookupServerById(_serverTypeId);
            if (server != null)
            {
                CommLog.info("CrossRankServerMgr regServer duplicate cross-rank server registration:" + _serverTypeId + " !");
                _m_alServerList.remove(server);
            }

            CrossRankServerInfo serverInfo = new CrossRankServerInfo(_serverTypeId, this);
            _m_alServerList.add(serverInfo);

            //发送注册消息到平台服务器
            NPCommonServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.CROSS_RANK.ordinal(), _serverTypeId,
                    NP2CRS_R_Writer_001_BasicOp.make_007_ReqCrossRankServerInfo(), new CrossRankServerInitCallbackDealer(serverInfo));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 尝试获取合适的Cross-Game服务器
     * @param _tmpAddWeight
     * @return
     */
    public CrossRankServerInfo tryHandleServer(int _tmpAddWeight)
    {
        CrossRankServerInfo selectServer = null;

        _lock();

        try
        {
            CrossRankServerInfo tmpServer = null;
            long minWeight = Long.MAX_VALUE;

            // 从队列中寻找一个处理权重最低且还可处理用户的服务器
            for (CrossRankServerInfo crossRankServerInfo : _m_alServerList)
            {
                tmpServer = crossRankServerInfo;
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

            // 尝试由选中服务器处理用户
            if (null == selectServer || !selectServer.addHandleWeight(_tmpAddWeight))
                return null;
        } finally
        {
            _unlock();
        }

        // 返回对应服务器
        return selectServer;
    }

    /**
     * 降低权重
     * @param _weight 权重
     */
    public void reduceWeight(int _typeId, int _weight)
    {
        _lock();
        try{
            CrossRankServerInfo server = lookupServerById(_typeId);
            if (server == null)
                return;

            server.reduceHandleWeight(_weight);
        }finally{
            _unlock();
        }
    }
}
