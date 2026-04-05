package NPBusServer;

import NPCommon.DDAlert.DDAlert;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import WCGBasicBusServer._AWCGBasicBusServer;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum;

import java.nio.ByteBuffer;

/**************
 * 通用服务器对象
 *
 * @author Administrator
 *
 */
public class NPBusServer extends _AWCGBasicBusServer
{
    private static NPBusServer _g_instance = null;
    public static NPBusServer getInstance()
    {
        return _g_instance;
    }

    /**
     * 每个服务器通用的钉钉报警对象
     */
    private DDAlert _m_dlDDAlert;

    protected NPBusServer()
    {
        //设置全局变量
        _g_instance = this;

        _m_dlDDAlert = new DDAlert();
    }

    public DDAlert getDDAlert() {return _m_dlDDAlert;}

    /******************
     * 当总线服务器对应服务器处理对象处理了无法处理的消息的时候调用本函数
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealUndealSenderMsg(_AWCGBSReceiverListener _basicServerListener, ByteBuffer _buf)
    {
        //无特殊需要处理的消息
    }

    /******************
     * 当总线服务器对应服务器处理对象处理了无法处理的消息的时候调用本函数
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealUndealSenderRequest(_AWCGBSReceiverListener _basicServerListener, _IWCGBasicRequestCommiter _commiter, ByteBuffer _buf)
    {
        //无特殊需要处理的请求
    }

    /***************
     * 初始化接口函数，返回初始化成功或失败
     * @return
     */
    @Override
    protected boolean _init()
    {
        //钉钉预警初始化
        getDDAlert().setServer(this);
        return true;
    }

    /********************
     * 根据带入的标记，获取监控服务器请求本服务器的状态字符串
     * 空默认为全局状态
     * @return
     */
    @Override
    public String getMonitorServerStateStr(String _infoTag)
    {
        NPBusMonitorInfo monitorInfo = new NPBusMonitorInfo();
        monitorInfo.setTotalMemory(Runtime.getRuntime().totalMemory());
        monitorInfo.setMaxMemory(Runtime.getRuntime().maxMemory());
        monitorInfo.setFreeMemory(Runtime.getRuntime().freeMemory());
        monitorInfo.setVersion(NPVersion.getString());
        monitorInfo.setTimeZone(CommonFunc.getTimeZone().getDisplayName());
        monitorInfo.setServerType(NPEnum.EServerType.SINGLE);
        monitorInfo.setServerTypeId(9999);
        monitorInfo.setIp(getConf().getInternalIp());
        monitorInfo.setInnerPort(getConf().getPort());
        return monitorInfo.makeJsonObj().toString();
    }

	@Override
	public void onBusServerHandleChg(int arg0) 
	{
	}
}
