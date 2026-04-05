package PayCenter;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ISelectDBInterface;
import NPCommon.DDAlert.DDAlert;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import PayCenter.Conf.PayCenterConf;
import PayCenter.Conf.Server.PayServerConfMgr;
import PayCenter.Http.Core.PayCenterHttpServiceCore;
import PayCenter.PayServer.PayServer;
import PayDB.PayDBInitializer;

import java.util.ArrayList;
import java.util.List;

/**
 * PayCenter - 支付中心服务器主控类
 * 
 * 主要功能：
 * 1. 管理PayServer实例列表
 * 2. 提供统一的数据库访问管理
 * 3. 支付中心系统参数管理
 * 4. DD告警系统集成
 * 
 * 设计特点：
 * - 单例模式管理全局实例
 * - 实现数据库接口用于BM路由
 * - 支持多PayServer实例管理
 * 
 * 线程安全：通过单例模式保证线程安全
 */
public class PayCenter implements _ISelectDBInterface
{
    private static PayCenter _g_instance = new PayCenter();

    public static PayCenter getInstance()
    {
        return _g_instance;
    }

    // 数据库访问对象
    private BM _m_bm;
    // DD告警对象
    private DDAlert _m_ddAlert;
    // 所有PayServer实例列表
    private List<PayServer> _m_allServerList;
    // 数据锁
    private MutexAtom _m_mutex;

    protected PayCenter()
    {
        // 设置全局变量
        _g_instance = this;

        _m_bm = new BM(this);
        _m_ddAlert = new DDAlert();
        _m_allServerList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    public BM getBM()
    {
        return _m_bm;
    }

    public DDAlert getDDAlert() 
    {
        return _m_ddAlert;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public List<PayServer> getAllServerList()
    {
        _lock();
        try{
            return new ArrayList<>(_m_allServerList);
        }finally
        {
            _unlock();
        }
    }

    @Override
    public EDBTag switchDBTag(EDBTag _srcTag)
    {
        return _srcTag;
    }

    /**
     * 添加PayServer实例到管理列表
     * 
     * @param server 要添加的PayServer实例
     */
    public void initAddServer(PayServer server)
    {
        _m_mutex.lock();
        try
        {
            _m_allServerList.add(server);
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 初始化支付中心系统
     * 
     * 执行流程：
     * 1. 初始化服务器日志系统
     * 2. 检查并初始化配置文件
     * 3. 建立数据库连接
     * 4. 初始化数据库结构
     * 5. 加载系统参数配置
     * 
     * @return true=初始化成功, false=初始化失败
     */
    public boolean init()
    {
        ALServerLog.initALServerLog();

        // 检查配置
        if (!PayServerConfMgr.getInstance().init())
        {
            CommLog.error("PayCenter Conf Init Fail!!!");
            return false;
        }

        // 初始化数据库连接
        if (!PayDBInitializer.initConnections())
        {
            CommLog.error("Pay DB init connection Fail!!!");
            return false;
        }

        // 初始化数据库结构
        if (!PayDBInitializer.initDB())
        {
            CommLog.error("Pay DB init db Fail!!!");
            return false;
        }

        // 初始化系统参数
        if (!PCParams.getInstance().initFromDB())
        {
            CommLog.error("PCParams init Fail!!!");
            return false;
        }

        //开启http服务
        if (!PayCenterHttpServiceCore.getInstance().init(PayCenterConf.getInstance().getToPlatHttpPort()))
        {
            ALServerLog.Fatal("NP NPHSHttpServiceCore DB Init table Fail!!!");
            return false;
        }

        return true;
    }

    /**
     * 查询PayServer实例
     * 通过平台id和地区id
     */
    public PayServer lookupPayServer(int _platformId, int _areaId)
    {
        _lock();
        try{
            for (PayServer server : _m_allServerList)
            {
                if (server.getConf().getPlatformId() == _platformId && server.getConf().getPlatAreaId() == _areaId)
                {
                    return server;
                }
            }
            return null;
        }finally
        {
            _unlock();
        }
    }
}