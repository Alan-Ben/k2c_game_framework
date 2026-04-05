package NPPlatServer.NPPS_Listener;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NPPlatServer.NPGeneralListener._ANPPSBasicServerListener;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public class PS_MarryMatchServerListener extends _ANPPSBasicServerListener
{
    private static PS_MarryMatchServerListener _g_instance = null;

    public static PS_MarryMatchServerListener getObj()
    {
        return _g_instance;
    }

    public static boolean isLogin()
    {
        return _g_instance != null;
    }

    public static void sendCustMsg(_IALProtocolStructure _protocol)
    {
        if (null == _g_instance || null == _protocol)
            return;

        _g_instance.sendCustomMsg(_protocol);
    }

    public static void sendReq(_IALProtocolStructure _protocol)
    {
        if (null == _g_instance || null == _protocol)
            return;

        _g_instance.sendRequest(_protocol);
    }

    public static void sendReq(_IALProtocolStructure _protocol, _IWCGCallbackDealer _callback)
    {
        if (null == _g_instance || null == _protocol)
            return;

        _g_instance.sendRequest(_protocol, _callback);
    }

    public static void sendReq(_IALProtocolStructure _protocol, long _callbackSerialize)
    {
        if (null == _g_instance || null == _protocol)
            return;

        _g_instance.sendRequest(_protocol, _callbackSerialize);
    }

    public PS_MarryMatchServerListener()
    {
        super(EServerType.SINGLE, ENPSingleServerType.MARRY_MATCH.ordinal());
    }

    @Override
    public void login()
    {

        ALServerLog.Sys("Marry Match Server: " + getServerTypeId() + " Login!");
        _g_instance = this;
    }

    /*****************
     * 在服务器断开连接的时候处理的函数
     */
    @Override
    protected void _onDisconnect()
    {
        ALServerLog.Sys("Marry Match Server: " + getServerTypeId() + " Disconnect!");
        //设置全局变量
        _g_instance = null;
    }

    /**************
     * 接收的消息长度超出时调用的函数
     */
    @Override
    public void onBuffLengthOverSize(ByteBuffer _srcBuf, ByteBuffer _curReadingBuf)
    {
    }
}
