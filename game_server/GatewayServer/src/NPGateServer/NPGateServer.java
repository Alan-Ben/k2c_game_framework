package NPGateServer;

import ALServerLog.ALServerLog;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPGameRes.ServerVersionInfo;
import NPGateServer.GMCommand.Cmds.CmdServer;
import NPGateServer.NPGSCallBack.NPGS2PS_RB_Callback_RegGSServer;
import NPGateServer.NPGeneralListener.GSBroadMsgDispather;
import NPGateServer.NPGeneralListener.MsgDispather.NPGSGeneralMsgDispather;
import NPGateServer.NPGeneralListener.NPGSBasicServerListener;
import NPGateServer.NPGeneralListener.RequestDispather.NPGSGeneralRequestDispather;
import NPGateServer.USRefVersionMgr.USVersionInfo;
import NPGateServer.USRefVersionMgr.USVersionMgr;
import NPGateServer.USServerInfoListMgr.USServerIndexInfoListMgr;
import NPServerProtocolWriter.NP2PS.Msg.NP2PS_Writer_002_GSOp;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Msg.NP2US_B_Writer_001_BasicOp;
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
public class NPGateServer extends _ABasicServerObj
{
    private static NPGateServer _g_instance = null;

    public static NPGateServer getInstance()
    {
        return _g_instance;
    }

    /**
     * 区域索引
     */
    private int _m_iAreaIdx;

    protected NPGateServer()
    {
        _m_iAreaIdx = 0;

        //设置全局变量
        _g_instance = this;
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
        return EServerType.GATE.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return GateServerConf.getInstance().getGateTypeId();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

        //发送本服务器的注册协议
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_002_RegGS(
                        GateServerConf.getInstance().getAreaTag()
                        , GateServerConf.getInstance().getUserMaxCount()
                        , GateServerConf.getInstance().getUserHandleWeight()
                        , GateServerConf.getInstance().getConnectIp()
                        , GateServerConf.getInstance().getConnectPort())
                , new NPGS2PS_RB_Callback_RegGSServer());

        //广播给 US GateServer上线了
        broadcastMessage(EServerType.USER.ordinal(), NP2US_B_Writer_001_BasicOp.make_004_GSOnline(getServerTypeId()));
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
        NPGSGeneralMsgDispather.getInstance().DealProtocol(null, _msg);
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
        NPGSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
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
        //Login服务器暂时不需要对其他服务器进行处理
        EServerType serverType = EServerType.values()[_serverType];
        return new NPGSBasicServerListener(this, serverType, _serverTypeId);
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

        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(ENPGatewayServerAsynEnum.REF_RELOAD.ordinal()))
        {
            ALServerLog.Fatal("NP Gate Server init NPRefDataMgr Fail!!!");
            return false;
        }

        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //初始化作弊命令
        GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());

        USServerIndexInfoListMgr.getInstance().init();

        //钉钉预警初始化
        getDDAlert().setServer(this);

        return true;
    }


    /*****************
     * 减少处理的用户
     * @param _uid
     */
    public void reduceHandleUser(String _uid)
    {
        //发送消息
        sendCustomMsgToPlat(NP2PS_Writer_002_GSOp.make_001_ReduceHandleUser(_uid));
    }

    public void addHandleUser(String _uid)
    {
        //发送消息
        sendCustomMsgToPlat(NP2PS_Writer_002_GSOp.make_002_AddHandleUser(_uid));
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
        if (_serverType == EServerType.USER.ordinal())
        {
            //是US数据则检索数据对象
            USVersionInfo info = USVersionMgr.getInstance().getOrCreateUSRefVersionInfo(_serverId);

            GSBroadMsgDispather.getInstance().DealProtocol(info, _msg);
        } else if (_serverType == EServerType.SINGLE.ordinal() && _serverId == ENPSingleServerType.COMMON.ordinal())
        {
            GSBroadMsgDispather.getInstance().DealProtocol(null, _msg);
        }
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
        NPGSMonitorInfo monitorInfo = new NPGSMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.GATE);
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
