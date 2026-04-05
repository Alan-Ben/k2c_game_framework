package PayCenter.PayServer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPServerProtocolWriter.NP2US.Msg.NP2US_B_Writer_001_BasicOp;
import PayCenter.Conf.Server.PayServerConf;
import PayCenter.PayServer.GeneralListener.MsgDispatcher.PCSGeneralMsgDispatcher;
import PayCenter.PayServer.GeneralListener.PCSBasicServerListener;
import PayCenter.PayServer.GeneralListener.RequestDispather.PCSGeneralRequestDispather;
import PayCenter.PayServer.GeneralListener.RequestDispather.PayServerRequestCommitter;
import PayCenter.PayServer.Task.PayServerHSPlatformInfoSynTask;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer.WCGPSClientListener.WCGPSRequestCommiter;
import WCGBasicServer._AWCGBasicServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**
 * PayServer - 支付服务器实现类
 * 
 * 主要功能：
 * 1. 继承基础服务器框架功能
 * 2. 处理支付相关的消息分发
 * 3. 管理与平台服务器的通信
 * 4. 提供服务器监控信息
 * 
 * 设计特点：
 * - 基于WCG基础服务器架构
 * - 支持消息和请求双重分发模式
 * - 集成服务器监控和状态报告
 * 
 * 线程安全：基于基础框架的线程安全机制
 */
public class PayServer extends _AWCGBasicServer
{
    // 服务器配置对象
    private PayServerConf _m_conf;

    public PayServer(PayServerConf _conf)
    {
        _m_conf = _conf;
    }

    public PayServerConf getConf()
    {
        return _m_conf;
    }

    @Override
    public boolean isBSDirectlySend(int i, int i1)
    {
        return false;
    }

    @Override
    public int getServerType()
    {
        return EServerType.SINGLE.ordinal();
    }

    @Override
    public int getServerTypeId()
    {
        return ENPSingleServerType.PAY.ordinal();
    }

    @Override
    public void onPSRegSuc()
    {
        ALServerLog.Sys("Plat Server Reg Suc!");

        ALSynTaskManager.getInstance().regTask(new PayServerHSPlatformInfoSynTask(this));

        //广播通知US Pay上线
        broadcastMessage(EServerType.USER.ordinal(), NP2US_B_Writer_001_BasicOp.make_005_PayOnline());
    }

    @Override
    public void onPSDisconnect()
    {
        ALServerLog.Sys("Plat Server Disconnect!");
    }

    /**
     * 处理来自平台服务器的自定义消息
     * 
     * @param _msg 消息数据
     */
    @Override
    public void dealPlatServerCustomMsg(ByteBuffer _msg)
    {
        PCSGeneralMsgDispatcher.getInstance().DealProtocol(null, _msg);
    }

    /**
     * 处理来自平台服务器的请求消息
     * 
     * @param _commiter 请求提交器
     * @param _msg 请求数据
     */
    @Override
    public void dealPlatServerRequest(WCGPSRequestCommiter _commiter, ByteBuffer _msg)
    {
        PCSGeneralRequestDispather.getInstance().DealProtocol(new PayServerRequestCommitter(this,_commiter), _msg);
    }

    @Override
    public void dealUndealBusServerCustomMsg(int _busTypeId, ByteBuffer _msg)
    {
        // 暂未实现总线服务器自定义消息处理
    }

    @Override
    public void dealUndealBusServerRequest(int _busTypeId, _IWCGBasicRequestCommiter _commiter, ByteBuffer _msg)
    {
        // 暂未实现总线服务器请求处理
    }

    @Override
    public void dealBroadcastCustomMsg(int _serverType, int _serverId, ByteBuffer _msg)
    {

    }

    /**
     * 创建基础服务器接收监听器
     * 
     * @param _serverType 服务器类型
     * @param _serverTypeId 服务器类型ID
     * @return 服务器监听器实例
     */
    @Override
    public _AWCGBSReceiverListener createBasicServerReceiverListener(int _serverType, int _serverTypeId)
    {
        // 转化枚举类型，根据不同服务器进行不同的处理
        EServerType serverType = EServerType.values()[_serverType];
        return new PCSBasicServerListener(this, serverType, _serverTypeId);
    }

    @Override
    public void dealCacheBusMsgAlert(int i)
    {
        // 暂未实现缓存总线消息告警处理
    }

    @Override
    public void tryGetHandleBusServerFail()
    {
        // 暂未实现总线服务器处理失败重试
    }

    /**
     * 获取服务器监控状态信息
     * 
     * @param s 状态参数
     * @return JSON格式的监控信息
     */
    @Override
    public String getMonitorServerStateStr(String s)
    {
        PayMonitorInfo monitorInfo = new PayMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(EServerType.SINGLE);
        monitorInfo.setServerTypeId(ENPSingleServerType.PAY.ordinal());
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }

    /**
     * 服务器初始化方法
     * 
     * @return true=初始化成功, false=初始化失败
     */
    @Override
    protected boolean _init()
    {
        return true;
    }

    @Override
    protected void _onBusServerSenderDisconnect(int i)
    {
        // 暂未实现总线服务器发送器断开处理
    }

	@Override
	public void onBusServerHandleChg(int arg0) 
	{
	}
}