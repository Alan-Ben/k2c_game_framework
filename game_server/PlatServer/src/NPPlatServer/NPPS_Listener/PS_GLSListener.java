package NPPlatServer.NPPS_Listener;

import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import NPPlatServer.NPPlatServerMgr.NPPlatGLSMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_GLSListener extends _ANPPSBasicServerListener
{
    public PS_GLSListener(int _serverTypeId)
    {
        super(EServerType.GAME_LOGIC, _serverTypeId);
    }

    @Override
    public void login()
    {
        ALServerLog.Sys("Game-Logic Server: " + getServerTypeId() + " Login!");
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("Game-Logic Server: " + getServerTypeId() + " Disconnect!");

        //注销
        NPPlatGLSMgr.getInstance().unReg(this);
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
