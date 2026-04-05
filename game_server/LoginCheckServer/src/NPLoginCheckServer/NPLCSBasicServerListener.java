package NPLoginCheckServer;

import ALServerLog.ALServerLog;
import NPLoginCheckServer.NPLCSMsgDealer.NPLCSMsgDispatcher;
import NPLoginCheckServer.NPLCSRequestDealer.NPLCSRequestDispather;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer._AWCGBasicServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class NPLCSBasicServerListener extends _AWCGBSReceiverListener
{
    public NPLCSBasicServerListener(_AWCGBasicServer _server, int _serverType, int _serverTypeId)
    {
        super(_server, _serverType, _serverTypeId);
    }

    @Override
    public void disconnect()
    {
        ALServerLog.Sys("Server: " + EServerType.values()[getServerType()] + " TypeId: " + getServerTypeId() + " Disconnected!");
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("Server: " + EServerType.values()[getServerType()] + " TypeId: " + getServerTypeId() + " Login!");
    }

    /******************
     * 处理发送方发送来的消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealSenderMsg(ByteBuffer _buf)
    {
        NPLCSMsgDispatcher.getInstance().DealProtocol(null, _buf);
    }

    /******************
     * 处理发送方发来的请求消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealSenderRequest(_IWCGBasicRequestCommiter _commiter, ByteBuffer _buf)
    {
        NPLCSRequestDispather.getInstance().DealProtocol(_commiter, _buf);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }

    /********************
     * 心跳包超时事件
     */
    @Override
    public void _onHeartMonitorTimeout()
    {
        ALServerLog.Error("Server: " + getServerType() + " - " + getServerTypeId() + "Heart Monitor Timeout");
    }
}
