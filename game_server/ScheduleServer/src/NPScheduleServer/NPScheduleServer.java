package NPScheduleServer;

import ALServerLog.ALServerLog;
import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.PHPParam.BSPHPParamMgr;
import NPCommon.Util.CommonFunc;
import NPCommon.Util._ABasicServerObj;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.CrossServerGroup.CrossServerGroupMgr;
import NPScheduleServer.GMCommand.Cmds.CmdServer;
import NPScheduleServer.NPGeneralListener.MsgDispather.NPSSGeneralMsgDispather;
import NPScheduleServer.NPGeneralListener.NPSSBasicServerListener;
import NPScheduleServer.NPGeneralListener.RequestDispather.NPSSGeneralRequestDispather;
import SSDB.ScheduleDBInitializer;
import SSLOGDB.ScheduleLogDBInitializer;
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
public class NPScheduleServer extends _ABasicServerObj
{
    private static NPScheduleServer _g_instance = null;
    private boolean _m_hadInit = false;//是否已经初始化成功

    protected NPScheduleServer()
    {
        super();

        //设置全局变量
        _g_instance = this;
    }

    public static NPScheduleServer getInstance()
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
        return ENPSingleServerType.SCHEDULE.ordinal();
    }

    /**********
     * 在成功注册时调用的函数
     */
    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

        if (!_m_hadInit)
        {
            _m_hadInit = true;

            //活动排期数据管理器，初始化完成处理
            ActivityScheduleDataMgr.getInstance().onInit();
        }
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
        NPSSGeneralMsgDispather.getInstance().DealProtocol(null, _msg);
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
        NPSSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
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
        return new NPSSBasicServerListener(this, serverType, _serverTypeId);
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

        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(ENPScheduleServerAsynEnum.REF_RELOAD.ordinal()))
        {
            ALServerLog.Fatal("NP Schedule Server init NPRefDataMgr Fail!!!");
            return false;
        }
        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        //数据库初始化
        if (!ScheduleDBInitializer.initConnections())//主库
        {
            ALServerLog.Fatal("NP Schedule Server Log DB Init connection Fail!!!");
            return false;
        }

        //初始化数据库表
        if (!ScheduleDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Schedule Server DB Init table Fail!!!");
            return false;
        }

        //日志数据库初始化
        if (!ScheduleLogDBInitializer.initConnections())//日志库
        {
            ALServerLog.Fatal("NP Schedule Server Log DB Init connection Fail!!!");
            return false;
        }

        //日志初始化数据库表
        if (!ScheduleLogDBInitializer.initDB())//主库
        {
            ALServerLog.Fatal("NP Schedule Server DB Init table Fail!!!");
            return false;
        }
        //NPISParams 初始化
        if (!NPSSParams.getInstance().initFromDB())
        {
            CommLog.fatal("can not init NPISParams!");
            return false;
        }

        //跨服服务器分组管理器 初始化
        if (!CrossServerGroupMgr.getInstance().initFromDB())
        {
            CommLog.fatal("can not init CrossServerGroupMgr!");
            return false;
        }

        //活动排期数据管理器 初始化
        if (!ActivityScheduleDataMgr.getInstance().initFromDB())
        {
            CommLog.fatal("can not init ActivityScheduleDataMgr!");
            return false;
        }

        //钉钉预警初始化
        getDDAlert().setServer(this);

        //数据库自动保存，只检测主库
        BoChecker.getInstance().startCheck(_bo -> _bo.getDBTag() == NPCommonEnum.EDBTag.ss_db);

        //初始化平台参数
        BSPHPParamMgr.getInstance().startLoad(this);

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
        NPSSMonitorInfo monitorInfo = new NPSSMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.SCHEDULE.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }
}
