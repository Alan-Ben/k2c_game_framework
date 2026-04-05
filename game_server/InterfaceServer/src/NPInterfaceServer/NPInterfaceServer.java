package NPInterfaceServer;

import ALServerLog.ALServerLog;
import ISDB.InterfaceDBInitializer;
import ISLOGDB.InterfaceLogDBInitializer;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;
import NPInterfaceServer.GMCommand.Cmds.CmdServer;
import NPInterfaceServer.NPGeneralListener.MsgDispather.NPISGeneralMsgDispather;
import NPInterfaceServer.NPGeneralListener.NPISBasicServerListener;
import NPInterfaceServer.NPGeneralListener.NPISParams;
import NPInterfaceServer.NPGeneralListener.RequestDispather.NPISGeneralRequestDispather;
import RPC.RpcSender;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**************
 * 登录服务器的服务器处理对象
 * @author Administrator
 *
 */
public class NPInterfaceServer extends _ABasicServerObj
{
    private static NPInterfaceServer _g_instance = null;
    public static NPInterfaceServer getInstance()
    {
        return _g_instance;
    }

    private RpcSender _m_rpc2UserServer = null;
    private RpcSender _m_rpc2CrossTeamServer = null;

    protected NPInterfaceServer()
    {
        //设置全局变量
        _g_instance = this;

        _m_rpc2UserServer = new RpcSender(this, EServerType.USER);
        _m_rpc2CrossTeamServer = new RpcSender(this, EServerType.SINGLE, ENPSingleServerType.CROSS_TEAM.ordinal());
    }

    public RpcSender rpc2us() {return _m_rpc2UserServer;}
    public RpcSender rpc2crossTeam() {return _m_rpc2CrossTeamServer;}

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
        return ENPSingleServerType.INTERFACE.ordinal();
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
        NPISGeneralMsgDispather.getInstance().DealProtocol(null, _msg);
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
        NPISGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
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
        
        return new NPISBasicServerListener(this, serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        //初始化聊天SDK配置
        if (!ChatSDKConf.getInstance().init())
        {
            ALServerLog.Fatal("ChatSDKConf Init Fail!!!");
            return false;
        }

        //初始化GM命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        //数据库初始化
        if (!InterfaceDBInitializer.initConnections())//主库
        {
            ALServerLog.Fatal("NP Interface Server Log DB Init connection Fail!!!");
            return false;
        }

        //初始化数据库表
        if (!InterfaceDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Interface Server DB Init table Fail!!!");
            return false;
        }
        //日志数据库初始化
        if (!InterfaceLogDBInitializer.initConnections())//日志库
        {
            ALServerLog.Fatal("NP Interface Server Log DB Init connection Fail!!!");
            return false;
        }

        //日志初始化数据库表
        if (!InterfaceLogDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Interface Server DB Init table Fail!!!");
            return false;
        }
        //NPISParams 初始化
        if (!NPISParams.getInstance().initFromDB())
        {
            CommLog.fatal("can not init NPISParams!");
            return false;
        }
        //聊天房间管理初始化
        if (!ChatRoomMgr.getInstance().initFromDB())
        {
            CommLog.fatal("can not init NpChatRoomMgr!");
            return false;
        }
        
        //聊天SDK初始化
        ChatMgr.getInstance();

        //钉钉预警初始化
        getDDAlert().setServer(this);

        return true;
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
        NPISMonitorInfo monitorInfo = new NPISMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.INTERFACE.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }
}
