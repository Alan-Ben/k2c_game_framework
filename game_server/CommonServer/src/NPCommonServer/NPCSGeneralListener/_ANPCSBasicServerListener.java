package NPCommonServer.NPCSGeneralListener;

import NPCommonServer.NPCSGeneralListener.MsgDispather.NPCSGeneralMsgDispather;
import NPCommonServer.NPCSGeneralListener.RequestDispather.NPCSGeneralRequestDispather;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServer._AWCGBasicServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public abstract class _ANPCSBasicServerListener extends _AWCGBSReceiverListener
{
    public _ANPCSBasicServerListener(_AWCGBasicServer _server, EServerType _serverType, int _serverTypeId)
    {
        super(_server, _serverType.ordinal(), _serverTypeId);
    }

    /******************
     * 处理自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealSenderMsg(ByteBuffer _msg)
    {
        NPCSGeneralMsgDispather.getInstance().DealProtocol(this, _msg);
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
        NPCSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
    }
}
