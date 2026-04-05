package GameLogicServer.GeneralListener;

import ALServerLog.ALServerLog;
import WCGBasicServer._AWCGBasicServer;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class GeneralBasicServerListener extends _ABasicServerListener
{
    public GeneralBasicServerListener(_AWCGBasicServer _server, EServerType _serverType, int _serverTypeId)
    {
        super(_server, _serverType, _serverTypeId);
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
