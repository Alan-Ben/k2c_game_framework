package NPUSServer.NPGeneralListener;

import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USServer.RPCDispatcher._IUSRPCDealer;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class NPUSGeneralBasicServerListener extends _ANPUSBasicServerListener implements _IUSRPCDealer
{
    public NPUSGeneralBasicServerListener(NPUserServer _server, EServerType _serverType, int _serverTypeId)
    {
        super(_server, _serverType, _serverTypeId);
    }

    @Override
    public void disconnect()
    {
        USLog.sys(getUSServer(), "Server: " + getServerType() + " TypeId: " + getServerTypeId() + " Disconnected!");
    }

    @Override
    public void login()
    {
        USLog.sys(getUSServer(), "Server: " + getServerType() + " TypeId: " + getServerTypeId() + " Login!");
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
        USLog.error(getUSServer(), "Server: " + getServerType() + " - " + getServerTypeId() + "Heart Monitor Timeout");
    }
}
