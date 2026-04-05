package NPPlatServer.NPPS_Listener;

import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import WCGCommon.Enum.NPEnum;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_CDSListener extends _ANPPSBasicServerListener
{
    public PS_CDSListener()
    {
        super(EServerType.SINGLE, NPEnum.ENPSingleServerType.CROSS_DATA.ordinal());
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("CDS Server Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("CDS Server Disconnect!");
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
