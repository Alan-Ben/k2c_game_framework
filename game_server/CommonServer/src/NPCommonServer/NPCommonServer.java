package NPCommonServer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import CSDB.CommonDBInitializer;
import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPCommonServer.CSServerMgr.SynTask.NPCSSynClearServerMgrTask;
import NPCommonServer.CallBack.WCGRBDealerRegServer;
import NPCommonServer.CommonServerLoaderMgr.CsLoaderMgr;
import NPCommonServer.GMCommand.Cmds.CmdServer;
import NPCommonServer.GeneralV.GeneralVMgr;
import NPCommonServer.HandleServerMgr.HandleServerType;
import NPCommonServer.NPCSGeneralListener.MsgDispather.NPCSGeneralMsgDispather;
import NPCommonServer.NPCSGeneralListener.NPCSGeneralBasicServerListener;
import NPCommonServer.PHPParmMgr.CSPHPParamMgr;
import NPCommonServer.PlayerFreezeMgr.PlayerFreezeMgr;
import NPCommonServer.USServerListMgr.USServerListMgrCSInstance;
import NPCommonServer.WhiteAccMgr.CSWhiteAccMgr;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPGameRes.ServerVersionInfo;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Msg.NP2US_B_Writer_001_BasicOp;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**************
 * 通用服务器对象
 * @author Administrator
 *
 */
public class NPCommonServer extends _ABasicServerObj implements _IALSynTask
{
    private static NPCommonServer _g_instance = null;

    public static NPCommonServer getInstance()
    {
        return _g_instance;
    }

    private CsLoaderMgr _m_syncServerLoader;

    public CsLoaderMgr getServerLoader()
    {
        return _m_syncServerLoader;
    }

    protected NPCommonServer()
    {
        _m_syncServerLoader = new CsLoaderMgr();
        //开启任务
        ALSynTaskManager.getInstance().regTask(this, 60 * 1000);

        //设置全局变量
        _g_instance = this;
    }

    public boolean isServerReady()
    {
        return _m_syncServerLoader.isAllLoad();
    }

    /**
     * 返回服务器类型信息
     */
    @Override
    public int getServerType()
    {
        return EServerType.SINGLE.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return ENPSingleServerType.COMMON.ordinal();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");
        //发送注册消息到平台服务器
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_001_RegServer(), new WCGRBDealerRegServer());

        //服务器注册成功后，开启加载其它数据
        getServerLoader().startAsyncLoad();
        //广播给 US CommonServer上线了
        broadcastMessage(EServerType.USER.ordinal(), NP2US_B_Writer_001_BasicOp.make_001_CSOnline());

        //发起加载白名单
        CSWhiteAccMgr.getInstance().startLoad();
    }

    /**********
     * 在从平台服务器断开连接时的事件函数
     */
    @Override
    public void onPSDisconnect()
    {
        ALServerLog.Sys("Plat Server Disconnect!");
        //从平台断开时清空区域信息
        ALSynTaskManager.getInstance().regTask(new NPCSSynClearServerMgrTask());
    }

    /******************
     * 处理平台服务器的自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealPlatServerCustomMsg(ByteBuffer _msg)
    {
        NPCSGeneralMsgDispather.getInstance().DealProtocol(null, _msg);
    }

    /******************
     * 处理平台服务器的请求
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealPlatServerRequest(WCGPSRequestCommiter _commiter, ByteBuffer _msg)
    {
    }

    /******************
     * 根据对应服务器类型构造接受协议的处理对象的函数
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public _AWCGBSReceiverListener createBasicServerReceiverListener(int _serverType, int _serverTypeId)
    {
        //转化枚举类型，根据不同服务器进行不同的处理
        //用户服务器只针对连接服务器进行处理和监听
        EServerType serverType = EServerType.values()[_serverType];
        return new NPCSGeneralBasicServerListener(this, serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        if (!ServerVersionInfo.getInstance().initLoadVersion())
        {
            CommLog.fatal("can not load game res version!");
            return false;
        }
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        CommLog.info("Game Res version:" + ServerVersionInfo.getInstance().getVersion());
        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(EWCGCommonServerAsynEnum.REF_RELOAD.ordinal()))
        {
            ALServerLog.Fatal("WCG Common Server init NPRefDataMgr Fail!!!");
            return false;
        }

        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //初始化数据库连接
        if (!CommonDBInitializer.initConnections())
        {
            ALServerLog.Fatal("WCG Common Server DB Init connection Fail!!!");
            return false;
        }
        //初始化数据库表 
        if (!CommonDBInitializer.initDB())
        {
            ALServerLog.Fatal("WCG Common Server DB Init table Fail!!!");
            return false;
        }
        //初始化参数表
        if (!NPCommParams.getInstance().initFromDB())
        {
            ALServerLog.Fatal("WCGCommParams.getInstance().initFromDB Fail!!!");
            return false;
        }
        if (!PlayerFreezeMgr.getInstance().initFromDB())
        {
            ALServerLog.Fatal("PlayerFreezeMgr.getInstance().initFromDB Fail!!!");
            return false;
        }

        //全局ID数据
        if (!GeneralVMgr.getInstance().s_init())
        {
            ALServerLog.Fatal("GeneralVMgr.getInstance().initFromDB Fail!!!");
            return false;
        }

        //初始化服务器负载类型管理器
        if(!HandleServerType.getInstance().sInit())
        {
            ALServerLog.Fatal("HandleServerType.getInstance().sInit Fail!!!");
            return false;
        }
        
        //发起加载服务器列表
        if(!USServerListMgrCSInstance.getInstance().startLoad())
        {
            ALServerLog.Fatal("USServerListMgrCSInstance.getInstance().startLoad Fail!!!");
            return false;
        }

        //数据库自动保存，只检测Common主库
        BoChecker.getInstance().startCheck(_bo -> _bo.getDBTag() == NPCommonEnum.EDBTag.comm_main);

        //打印服务器参数
        NPCommParams.getInstance().dump();

        //钉钉预警初始化
        getDDAlert().setServer(this);
        
        //发起加载平台参数
        CSPHPParamMgr.getInstance().startLoad();

        return true;
    }

    @Override
    public void run()
    {
        try
        {
            long mb = 1024 * 1024;
            long totalMem = Runtime.getRuntime().totalMemory() / mb;
            long freeMem = Runtime.getRuntime().freeMemory() / mb;
            long maxMem = Runtime.getRuntime().maxMemory() / mb;

            CommLog.info("[SYSINFO][mem]:total[{}MB],free[{}MB],maxavail[{}MB]"
                    , totalMem
                    , freeMem
                    , maxMem);
        } finally
        {

            ALSynTaskManager.getInstance().regTask(this, 60 * 1000);
        }
    }

    /******************
     * 处理平台服务器广播的自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealBroadcastCustomMsg(int _serverType, int _serverId, ByteBuffer _msg)
    {
    }

    /*****************
     * 是否直接发送消息到BasicServer，建议默认都使用false
     * @return
     */
    @Override
    public boolean isBSDirectlySend(int _serverType, int _serverTypeId)
    {
        return false;
    }

    /*******************
     * 缓存的总线消息数量过大的时候触发的报警处理
     * @param _msgCount
     */
    @Override
    public void dealCacheBusMsgAlert(int _msgCount)
    {
        ALServerLog.Fatal("Bus Server Msg Cache for [" + _msgCount + "]");
    }

    /*******************
     * 尝试获取负载本服的总线服务器失败
     */
    @Override
    public void tryGetHandleBusServerFail()
    {
        ALServerLog.Fatal("Try Get Handle Bus Server Fail!!!");
    }

    /*******************
     * 在发送请求到总线服务器的连接对象断开连接的时候的处理函数
     */
    @Override
    protected void _onBusServerSenderDisconnect(int _serverTypeId)
    {
        ALServerLog.Fatal("Bus Server [" + _serverTypeId + "] Disconnect!!!");
    }

    /******************
     * 处理平台服务器的自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealUndealBusServerCustomMsg(int _busTypeId, ByteBuffer _msg)
    {

    }

    /******************
     * 处理平台服务器的请求
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealUndealBusServerRequest(int _busTypeId, _IWCGBasicRequestCommiter _commiter, ByteBuffer _msg)
    {

    }

    /********************
     * 根据带入的标记，获取监控服务器请求本服务器的状态字符串
     * 空默认为全局状态
     * @return
     */
    @Override
    public String getMonitorServerStateStr(String _infoTag)
    {
        NPCSMonitorInfo monitorInfo = new NPCSMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.COMMON.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        monitorInfo.setRefVersion(ServerVersionInfo.getInstance().getVersion());
        monitorInfo.setReadyRefVersion(NPRefDataMgr.getInstance().getRefDataReloader().getVersion());
        monitorInfo.setIsReadyRefLoading(NPRefDataMgr.getInstance().getRefDataReloader().isLoading());
        monitorInfo.setIsReadyRefReload(NPRefDataMgr.getInstance().getRefDataReloader().isReadyReload());
        return monitorInfo.makeJsonObj().toString();
    }

    /******
     * 返回本服所在的分区，欧洲区，亚洲区。。。。
     * @return
     */
    public long getServerAreaId()
    {
        //TODO:完善分区读取
        return 1;
    }
}
