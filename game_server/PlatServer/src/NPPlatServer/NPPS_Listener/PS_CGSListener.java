package NPPlatServer.NPPS_Listener;

import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import NPPlatServer.NPPlatServerMgr.NPPlatCGSMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_CGSListener extends _ANPPSBasicServerListener
{
    public PS_CGSListener(int _serverTypeId)
    {
        super(EServerType.CROSS_GAME, _serverTypeId);
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("Cross-Game Server: " + getServerTypeId() + " Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("Cross-Game Server: " + getServerTypeId() + " Disconnect!");

        //注销
        NPPlatCGSMgr.getInstance().unregCGS(this);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
