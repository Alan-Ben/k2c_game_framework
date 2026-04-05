package NPHttpServer;

import ALServerLog.ALServerLog;
import HSDB.HttpDBInitializer;
import HSLOGDB.HttpLogDBInitializer;
import NP2CS.p001_BasicOp.NP2CS_001_008_OnHSOnline;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPGameRes.ServerVersionInfo;
import NPHttpServer.GMCommand.Cmds.CmdServer;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.NPGeneralListener.MsgDispather.NPHSGeneralMsgDispather;
import NPHttpServer.NPGeneralListener.NPHSBasicServerListener;
import NPHttpServer.NPGeneralListener.RequestDispather.NPHSGeneralRequestDispather;
import NPHttpServer.NPHSAllServerMail.NPHSAllServerMailMgr;
import NPHttpServer.NPWhiteAccMgr.NPWhiteAccMgr;
import RPC.RpcSender;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**************
 * 处理http请求服务器的服务器处理对象
 * @author Administrator
 *
 */
public class NPHttpServer extends _ABasicServerObj
{
    private static NPHttpServer _g_instance = null;

    public RpcSender rpc2us;

    protected NPHttpServer()
    {
        rpc2us = new RpcSender(this, EServerType.USER);

        //设置全局变量
        _g_instance = this;
    }

    public static NPHttpServer getInstance()
    {
        return _g_instance;
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
        return ENPSingleServerType.HTTP.ordinal();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

        sendMessageToBSServer(EServerType.SINGLE.ordinal(),ENPSingleServerType.COMMON.ordinal(),new NP2CS_001_008_OnHSOnline());
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
        NPHSGeneralMsgDispather.getInstance().DealProtocol(null, _msg);
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
        NPHSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
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
        return new NPHSBasicServerListener(this, serverType, _serverTypeId);
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

        //初始化GM命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        //数据库初始化
        if (!HttpDBInitializer.initConnections())//主库
        {
            ALServerLog.Fatal("NP Http Server Log DB Init connection Fail!!!");
            return false;
        }

        //初始化数据库表
        if (!HttpDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Http Server DB Init table Fail!!!");
            return false;
        }
        //日志数据库初始化
        if (!HttpLogDBInitializer.initConnections())//日志库
        {
            ALServerLog.Fatal("NP Http Server Log DB Init connection Fail!!!");
            return false;
        }

        //日志初始化数据库表
        if (!HttpLogDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Http Server DB Init table Fail!!!");
            return false;
        }

        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(ENPHttpServerAsynEnum.REF_RELOAD.ordinal()))
        {
            ALServerLog.Fatal("NP Http Server init NPRefDataMgr Fail!!!");
            return false;
        }

        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //初始化服务器参数
        if (!HSParams.getInstance().initFromDB())
        {
            ALServerLog.Fatal("HSParams DB Init table Fail!!!");
            return false;
        }

        //开启http服务
        if (!NPHSHttpServiceCore.getInstance().init(HttpServerConf.getInstance().getToPlatHttpPort()))
        {
            ALServerLog.Fatal("NP NPHSHttpServiceCore DB Init table Fail!!!");
            return false;
        }

        //初始化白名单	
        if (!NPWhiteAccMgr.getInstance().initFromDB())
        {
            ALServerLog.Fatal("NP NPWhiteAccMgr DB Init table Fail!!!");
            return false;
        }

        //初始化全服邮件数据
        if (!NPHSAllServerMailMgr.getInstance().initFromDB())
        {
            ALServerLog.Fatal("NP NPHSAllServerMailMgr DB Init table Fail!!!");
            return false;
        }

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
        NPHSMonitorInfo monitorInfo = new NPHSMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.HTTP.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        monitorInfo.setRefVersion(ServerVersionInfo.getInstance().getVersion());
        monitorInfo.setReadyRefVersion(NPRefDataMgr.getInstance().getRefDataReloader().getVersion());
        monitorInfo.setIsReadyRefLoading(NPRefDataMgr.getInstance().getRefDataReloader().isLoading());
        monitorInfo.setIsReadyRefReload(NPRefDataMgr.getInstance().getRefDataReloader().isReadyReload());
        return monitorInfo.makeJsonObj().toString();
    }
}
