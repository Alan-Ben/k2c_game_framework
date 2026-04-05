package NPPlatServer.NPPS_Listener;

import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_CRSListener extends _ANPPSBasicServerListener
{
    public PS_CRSListener(int _serverTypeId)
    {
        super(EServerType.CROSS_RANK, _serverTypeId);
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("WCG Cross Rank Server: " + getServerTypeId() + " Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("Cross Rank Server: " + getServerTypeId() + " Disconnect!");
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
