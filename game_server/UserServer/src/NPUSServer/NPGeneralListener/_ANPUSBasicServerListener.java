package NPUSServer.NPGeneralListener;

import NPUSServer.NPUserServer;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public abstract class _ANPUSBasicServerListener extends _AWCGBSReceiverListener
{
    private NPUserServer _m_usServer;

    public _ANPUSBasicServerListener(NPUserServer _server, EServerType _serverType, int _serverTypeId)
    {
        super(_server, _serverType.ordinal(), _serverTypeId);

        _m_usServer = _server;
    }

    public NPUserServer getUSServer() { return _m_usServer;}

    /******************
     * 处理自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealSenderMsg(ByteBuffer _msg)
    {
        _m_usServer.getGeneralMsgDispather().DealProtocol(this, _msg);
    }

    /******************
     * 处理请求
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealSenderRequest(_IWCGBasicRequestCommiter _commiter, ByteBuffer _msg)
    {
        getUSServer().getGeneralRequestDispather().DealProtocol(_commiter, _msg);
    }
}
