package MarryMatchServer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import MarryMatchServer.GMCommand.Cmds.CmdServer;
import MarryMatchServer.LoaderServerMgr.MarryMatchLoaderMgr;
import MarryMatchServer.MarryMatchMgr.MarryMatchMgr;
import MarryMatchServer.NPGeneralListener.MarryMatchBasicServerListener;
import MarryMatchServer.NPGeneralListener.MsgDispather.MarryMatchGeneralMsgDispather;
import MarryMatchServer.NPGeneralListener.RequestDispather.MarryMatchGeneralRequestDispather;
import NP2US_B.p001_BasicOp.NP2US_B_001_009_MatchAdultServerOnline;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import RPC.RpcSender;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**************
 * 子嗣联姻服务器的服务器处理对象
 * @author Administrator
 *
 */
public class MarryMatchServer extends _ABasicServerObj implements _IALSynTask
{
    private static MarryMatchServer _g_instance = null;
    public static MarryMatchServer getInstance()
    {
        return _g_instance;
    }
    
    //US通信RPC对象
    public RpcSender rpc2us;
    //服务器加载管理对象
    private MarryMatchLoaderMgr _m_syncServerLoader;

    protected MarryMatchServer()
    {
        _m_syncServerLoader = new MarryMatchLoaderMgr();
        //开启任务
        ALSynTaskManager.getInstance().regTask(this, 60 * 1000);
    	
        //设置全局变量
        _g_instance = this;
        
        rpc2us = new RpcSender(this, EServerType.USER);
    }

    public MarryMatchLoaderMgr getServerLoader()
    {
        return _m_syncServerLoader;
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
        return ENPSingleServerType.MARRY_MATCH.ordinal();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

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
        MarryMatchGeneralMsgDispather.getInstance().DealProtocol(null, _msg);
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
        MarryMatchGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
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
        EServerType serverType = EServerType.values()[_serverType];
        return new MarryMatchBasicServerListener(this, serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        //初始化GM命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        //全服联姻池数据初始化
        if(!MarryMatchMgr.getInstance().init())
        {
            ALServerLog.Fatal("MarryMatchMgr Init table Fail!!!");
            return false;
        }
        
        //通知所有US服务器
        getServerLoader().safeCall(() -> broadcastMessage(EServerType.USER.ordinal(),new NP2US_B_001_009_MatchAdultServerOnline()));

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
        } 
        finally
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
        MarryMatchMonitorInfo monitorInfo = new MarryMatchMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.MARRY_MATCH.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }
}
