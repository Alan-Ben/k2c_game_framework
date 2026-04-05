package GameLogicServer;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import GLSDB.GameLogicDBInitializer;
import GLSLOGDB.GameLogicLogDBInitializer;
import GameLogicServer.Callback.RBDealerRegServer;
import GameLogicServer.Cmd.CmdServer;
import GameLogicServer.GeneralListener.GeneralBasicServerListener;
import GameLogicServer.GroupMgr.ActivityFactory.ActivityFactory;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPGameRes.ServerVersionInfo;
import NPServerProtocolWriter.NP2US.Msg.NP2US_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_001_BasicOp;
import RPC.RpcSender;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.HashMap;
import java.nio.ByteBuffer;

/**************
 * 排行榜数据服务器对象
 *
 * @author Administrator
 *
 */
public class GameLogicServer extends _ABasicServerObj implements _IALSynTask
{
    private static GameLogicServer _g_instance = null;
    public static GameLogicServer getInstance()
    {
        return _g_instance;
    }

    //US服务器的RPC发送对象
    private RpcSender _m_rpc2US = null;
    //CTS服务器RPC发送对象
    private RpcSender _m_rpc2CTServer = null;

    protected GameLogicServer()
    {
        //设置全局变量
        _g_instance = this;

        _m_rpc2US = new RpcSender(this, EServerType.USER);
        _m_rpc2CTServer = new RpcSender(this, EServerType.SINGLE, NPEnum.ENPSingleServerType.CROSS_TEAM.ordinal());

        //开启任务
        ALSynTaskManager.getInstance().regTask(this, 60 * 1000);
    }

    /**
     * 返回服务器类型信息
     */
    @Override
    public int getServerType()
    {
        return EServerType.GAME_LOGIC.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return GameLogicServerConf.getInstance().getTypeId();
    }

    public RpcSender rpc2us() {return _m_rpc2US;}
    public RpcSender rpc2cts() {return _m_rpc2CTServer;}

    /**
     * 给指定cid玩家发送消息
     */
    public void sendMsg2GC(long _cid, _IALProtocolStructure _protocol)
    {
        if (null == _protocol)
            return;

        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);
        sendMessageToBSServer(EServerType.USER.ordinal(), usId,
                NP2US_Writer_001_BasicOp.make_002_SendbackUserMsg(_cid, _protocol));
    }

    /**
     * 对指定cid列表广播消息，按US分组后发送
     */
    public void broadMsg2GC(ArrayList<Long> _cidList, _IALProtocolStructure _protocol)
    {
        if (null == _protocol || null == _cidList || _cidList.isEmpty())
            return;

        HashMap<Integer, ArrayList<Long>> usCidMap = new HashMap<>();
        for (long cid : _cidList)
        {
            int usId = CommonFunc.parseServerTypeIdFromCid(cid);
            usCidMap.computeIfAbsent(usId, k -> new ArrayList<>()).add(cid);
        }

        ByteBuffer msgBuffer = _protocol.makeFullPackage();
        for (HashMap.Entry<Integer, ArrayList<Long>> entry : usCidMap.entrySet())
        {
            sendMessageToBSServer(EServerType.USER.ordinal(), entry.getKey(),
                    NP2US_Writer_001_BasicOp.make_004_BroadUserMsg(entry.getValue(), msgBuffer));
        }
    }

    /**
     * 向指定玩家远程触发逻辑事件
     */
    public void triggerPlayerEvent(long _cid, String _eventName, ArrayList<Long> _paramList)
    {
        if (null == _eventName || _eventName.isEmpty())
            return;

        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);
        sendMessageToBSServer(EServerType.USER.ordinal(), usId,
                NP2US_Writer_001_BasicOp.make_005_TriggerPlayerEvent(_cid, _eventName, _paramList));
    }

    /**
     * 向指定cid列表广播远程触发逻辑事件，按US分组后发送
     */
    public void broadTriggerPlayerEvent(ArrayList<Long> _cidList, String _eventName, ArrayList<Long> _paramList)
    {
        if (null == _eventName || _eventName.isEmpty() || null == _cidList || _cidList.isEmpty())
            return;

        HashMap<Integer, ArrayList<Long>> usCidMap = new HashMap<>();
        for (long cid : _cidList)
        {
            int usId = CommonFunc.parseServerTypeIdFromCid(cid);
            usCidMap.computeIfAbsent(usId, k -> new ArrayList<>()).add(cid);
        }

        for (HashMap.Entry<Integer, ArrayList<Long>> entry : usCidMap.entrySet())
        {
            sendMessageToBSServer(EServerType.USER.ordinal(), entry.getKey(),
                    NP2US_Writer_001_BasicOp.make_006_BroadTriggerPlayerEvent(entry.getValue(), _eventName, _paramList));
        }
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

        // 发送注册消息到平台服务器
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_014_RegGameLogicServer(), new RBDealerRegServer());
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
        return new GeneralBasicServerListener(this, serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(EGameLogicServerAsynEnum.REF_RELOAD.ordinal()))
        {
            ALServerLog.Fatal("Game-Logic Server init NPRefDataMgr Fail!!!");
            return false;
        }
        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //初始化数据库连接
        if (!GameLogicDBInitializer.initConnections())//主库
        {
            ALServerLog.Fatal("Game-Logic Server DB Init connection Fail!!!");
            return false;
        }
        //初始化数据库表
        if (!GameLogicDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("Game-Logic Server DB Init table Fail!!!");
            return false;
        }

        //初始化日志库
        if (!GameLogicLogDBInitializer.initConnections())//日志库
        {
            ALServerLog.Fatal("Game-Logic Server LogDB Init connection Fail!!!");
            return false;
        }
        //初始化数据库表
        if (!GameLogicLogDBInitializer.initDB())//日志库
        {
            ALServerLog.Fatal("Game-Logic Server LogDB Init table Fail!!!");
            return false;
        }

        //加载活动工厂数据
        if(!ActivityFactory.getInstance().sInit())
        {
            ALServerLog.Fatal("ActivityFactory Init Fail!!!");
            return false;
        }

        //加载分组实例数据
        if(!GroupInstanceMgr.getInstance().sInit())
        {
            ALServerLog.Fatal("GroupInstanceMgr Init Fail!!!");
            return false;
        }

        //GM命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

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
     * @param _serverTypeId
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
        GameLogicServerMonitorInfo monitorInfo = new GameLogicServerMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(getServerTypeId());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        monitorInfo.setRefVersion(ServerVersionInfo.getInstance().getVersion());
        monitorInfo.setReadyRefVersion(NPRefDataMgr.getInstance().getRefDataReloader().getVersion());
        monitorInfo.setIsReadyRefLoading(NPRefDataMgr.getInstance().getRefDataReloader().isLoading());
        monitorInfo.setIsReadyRefReload(NPRefDataMgr.getInstance().getRefDataReloader().isReadyReload());
        return monitorInfo.makeJsonObj().toString();
    }
}
