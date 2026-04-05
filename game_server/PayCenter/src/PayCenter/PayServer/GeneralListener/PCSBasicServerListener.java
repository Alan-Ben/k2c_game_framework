package PayCenter.PayServer.GeneralListener;

import ALServerLog.ALServerLog;
import PayCenter.PayServer.GeneralListener.MsgDispatcher.PCSGeneralMsgDispatcher;
import PayCenter.PayServer.GeneralListener.RequestDispather.PCSGeneralRequestDispather;
import PayCenter.PayServer.GeneralListener.RequestDispather.PayServerRequestCommitter;
import PayCenter.PayServer.PayServer;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer._AWCGBasicServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

/**
 * PCSBasicServerListener - PayCenter基础服务器监听器
 * 
 * 主要功能：
 * 1. 处理来自其他服务器的连接和消息
 * 2. 根据服务器类型进行不同的处理逻辑
 * 3. 管理服务器间的通信协议
 * 4. 提供服务器连接状态监控
 * 
 * 设计特点：
 * - 继承WCG基础监听器框架
 * - 支持多种服务器类型处理
 * - 消息路由和分发机制
 * - 连接状态管理
 */
public class PCSBasicServerListener extends _AWCGBSReceiverListener
{
    public PCSBasicServerListener(_AWCGBasicServer _server, EServerType _serverType, int _serverTypeId)
    {
        super(_server, _serverType.ordinal(), _serverTypeId);
    }

    @Override
    public void disconnect()
    {
        ALServerLog.Sys("Server: " + getServerType() + " TypeId: " + getServerTypeId() + " Disconnected!");
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("Server: " + getServerType() + " TypeId: " + getServerTypeId() + " Login!");
    }

    /**
     * 处理发送方发送来的消息
     * 
     * @param _buf 消息数据缓冲区
     */
    @Override
    public void dealSenderMsg(ByteBuffer _buf)
    {
        PCSGeneralMsgDispatcher.getInstance().DealProtocol(this, _buf);
    }

    /**
     * 处理发送方发来的请求消息
     * 
     * @param _commiter 请求提交器
     * @param _buf 请求数据缓冲区
     */
    @Override
    public void dealSenderRequest(_IWCGBasicRequestCommiter _commiter, ByteBuffer _buf)
    {
        PayServer server = (PayServer) getBasicServer();
        PCSGeneralRequestDispather.getInstance().DealProtocol(new PayServerRequestCommitter(server,_commiter), _buf);
    }

    /**
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }

    /**
     * 心跳包超时事件
     */
    @Override
    public void _onHeartMonitorTimeout()
    {
        ALServerLog.Error("Server: " + getServerType() + " - " + getServerTypeId() + "Heart Monitor Timeout");
    }
}