package NPGateServer.NPGeneralListener;

import ALServerLog.ALServerLog;
import NPGateServer.NPGeneralListener.MsgDispather.NPGSGeneralMsgDispather;
import NPGateServer.NPGeneralListener.RequestDispather.NPGSGeneralRequestDispather;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer._AWCGBasicServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class NPGSBasicServerListener extends _AWCGBSReceiverListener
{
    public NPGSBasicServerListener(_AWCGBasicServer _server, EServerType _serverType, int _serverTypeId)
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

    /******************
     * 处理发送方发送来的消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealSenderMsg(ByteBuffer _buf)
    {
        NPGSGeneralMsgDispather.getInstance().DealProtocol(this, _buf);
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
        NPGSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _buf);
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
