package NPLoginServer;

import ALServerLog.ALServerLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPLoginServer.NPGeneralListener.MsgDispather.NP2LSGeneralMsgDispatcher;
import NPLoginServer.NPGeneralListener.NPLSBasicServerListener;
import NPLoginServer.NPGeneralListener.RequsetDispather.NP2LSGeneralRequestDispatcher;
import NPLoginServer.PSRequestCallBack.NP_PS_RBDealerRegServer;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_001_BasicOp;
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
public class NPLoginServer extends _ABasicServerObj
{
    private static NPLoginServer _g_instance = null;

    public static NPLoginServer getInstance()
    {
        return _g_instance;
    }

    /**
     * 区域索引
     */
    private int _m_iAreaIdx;
    /**
     * 是否服务器准备好了
     */
    private boolean _m_bIsServerReady = false;
    /**
     * 服务器是否屏蔽正常登陆
     */
    private boolean _m_bIsForbidenLogin = false;

    //登陆检测
    private RpcSender _m_Rpc2LCSServer = null;

    public RpcSender rpc2lcs()
    {
        return _m_Rpc2LCSServer;
    }

    //通用服务
    private RpcSender _m_Rpc2CSSserver = null;

    public RpcSender rpc2cs()
    {
        return _m_Rpc2CSSserver;
    }

    protected NPLoginServer()
    {
        _m_iAreaIdx = 0;
        _m_Rpc2LCSServer = new RpcSender(this, EServerType.SINGLE, ENPSingleServerType.LOGIN_CHECK.ordinal());
        _m_Rpc2CSSserver = new RpcSender(this, EServerType.SINGLE, ENPSingleServerType.COMMON.ordinal());

        //设置全局变量
        _g_instance = this;
    }

    public boolean isServerReady()
    {
        return _m_bIsServerReady;
    }

    public boolean isForbidenLogin()
    {
        return _m_bIsForbidenLogin;
    }

    public void setIsForbidenLogin(boolean _isForbidenLogin)
    {
        _m_bIsForbidenLogin = _isForbidenLogin;
    }

    /**
     * 区域标记索引的相关信息
     */
    public int getAreaIdx()
    {
        return _m_iAreaIdx;
    }

    public void initAreaIdx(int _areaIdx)
    {
        _m_iAreaIdx = _areaIdx;
    }

    /**
     * 返回服务器类型信息
     */
    @Override
    public int getServerType()
    {
        return EServerType.LOGIN.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return LoginServerConf.getInstance().getLoginTypeId();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");
        //发送本服务器的注册协议
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_004_RegLSServer(LoginServerConf.getInstance().getAreaTag()), new NP_PS_RBDealerRegServer());

        //设置服务器准备好
        _m_bIsServerReady = true;
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
        NP2LSGeneralMsgDispatcher.getInstance().DealProtocol(null, _msg);
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
        NP2LSGeneralRequestDispatcher.getInstance().DealProtocol(_commiter, _msg);
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
        return new NPLSBasicServerListener(this, _serverType, _serverTypeId);
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
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
        NPLSMonitorInfo monitorInfo = new NPLSMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.LOGIN);
        monitorInfo.setServerTypeId(getServerTypeId());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }
}
