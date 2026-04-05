package NPPlatServer.NPPS_Listener;

import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import NPPlatServer.NPPlatServerMgr.NPPlatUSMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_USListener extends _ANPPSBasicServerListener
{
    public PS_USListener(int _serverTypeId)
    {
        super(EServerType.USER, _serverTypeId);
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("WCG User Server: " + getServerTypeId() + " Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("User Server: " + getServerTypeId() + " Disconnect!");

        //注销服务器的区域类型
        NPPlatUSMgr.getInstance().unregUS(this);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
