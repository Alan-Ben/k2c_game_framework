package NPCrossGameServer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import CGSDB.CrossGameDBInitializer;
import CGSLOGDB.CrossGameLogDBInitializer;
import NPCommon.CommonCache.Player.Getter.PlayerCacheGetter;
import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPCrossGameServer.CallBack.NPRBDealerRegServer;
import NPCrossGameServer.GMCommand.Cmd.CmdServer;
import NPCrossGameServer.GeneralV.GeneralVMgr;
import NPCrossGameServer.NPCrossGameCore.CrossGameCategoryMgr;
import NPCrossGameServer.NPGeneralListener.NPCGSGeneralBasicServerListener;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPGameRes.ServerVersionInfo;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_001_BasicOp;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**************
 * 排行榜数据服务器对象
 *
 * @author Administrator
 *
 */
public class NPCrossGameServer extends _ABasicServerObj implements _IALSynTask
{
    private static NPCrossGameServer _g_instance = null;

    public static NPCrossGameServer getInstance()
    {
        return _g_instance;
    }

    /**
     * 区域编号
     */
    private int _m_iAreaIndex;
    private PlayerCacheGetter _m_pcgPlayerCacheGetter;

    protected NPCrossGameServer()
    {
        //设置全局变量
        _g_instance = this;

        _m_pcgPlayerCacheGetter = new PlayerCacheGetter(this);

        //开启任务
        ALSynTaskManager.getInstance().regTask(this, 60 * 1000);
    }

    public PlayerCacheGetter getPlayerCacheGetter() {return _m_pcgPlayerCacheGetter;}

    public int getAreaIndex()
    {
        return _m_iAreaIndex;
    }

    /**
     * 返回服务器类型信息
     */
    @Override
    public int getServerType()
    {
        return EServerType.CROSS_GAME.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return CrossGameServerConf.getInstance().getTypeId();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

        // 发送注册消息到平台服务器
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_010_RegCGS(CrossGameServerConf.getInstance().getAreaTag()), new NPRBDealerRegServer());
    }

    /**********
     * 在从平台服务器断开连接时的事件函数
     */
    @Override
    public void onPSDisconnect()
    {
        ALServerLog.Sys("Plat Server Disconnect!");
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
        return new NPCGSGeneralBasicServerListener(this, serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        //加载版本号数据
        if (!ServerVersionInfo.getInstance().initLoadVersion())
        {
            CommLog.fatal("can not load game res version!");
            return false;
        }
        CommLog.info("Game Res version:" + ServerVersionInfo.getInstance().getVersion());

        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(EWCGCrossGameServerAsynEnum.REF_RELOAD.ordinal()))
        {
            ALServerLog.Fatal("NP Cross-Game Server init NPRefDataMgr Fail!!!");
            return false;
        }
        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //初始化数据库连接
        if (!CrossGameDBInitializer.initConnections())//主库
        {
            ALServerLog.Fatal("NP Cross-Game Server DB Init connection Fail!!!");
            return false;
        }
        //初始化数据库表 
        if (!CrossGameDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Cross-Game Server DB Init table Fail!!!");
            return false;
        }

        //初始化日志库
        if (!CrossGameLogDBInitializer.initConnections())//日志库
        {
            ALServerLog.Fatal("NP Cross-Game Server LogDB Init connection Fail!!!");
            return false;
        }
        //初始化数据库表 
        if (!CrossGameLogDBInitializer.initDB())//日志库
        {
            ALServerLog.Fatal("NP Cross-Game Server LogDB Init table Fail!!!");
            return false;
        }

        //初始化服务器Id管理器
        if (!GeneralVMgr.getInstance().s_init())
        {
            CommLog.error("GeneralVMgr init failed!");
            return false;
        }

        //初始化跨服游戏数据
        if (!CrossGameCategoryMgr.getInstance().init())
        {
            CommLog.error("CrossGameCategoryMgr init failed!");
            return false;
        }

        //GM命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        //数据库自动保存，只检测Rank主库
        BoChecker.getInstance().startCheck(_bo -> _bo.getDBTag() == NPCommonEnum.EDBTag.crossgame_main);

        //同步当前服务器权重到CS
        ALSynTaskManager.getInstance().regTask(SynTask_CalCrossGameWeight.getInstance());

        //钉钉预警初始化
        getDDAlert().setServer(this);

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
        NPCGSMonitorInfo monitorInfo = new NPCGSMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.CROSS_GAME);
        monitorInfo.setServerTypeId(getServerTypeId());
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
    public void setAreaIndex(int _areaIndex)
    {
        _m_iAreaIndex = _areaIndex;
    }
}
