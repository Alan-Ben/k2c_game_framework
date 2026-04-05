package NPPlatServer.NPGeneralListener;

import NPPlatServer.NPGeneralListener.MsgDispather.NPPSGeneralMsgDispather;
import NPPlatServer.NPGeneralListener.RequsetDispather.NPPSGeneralRequestDispather;
import WCGBasicPlatServer.BasicServerListener.WCGBSRequestCommiter;
import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public abstract class _ANPPSBasicServerListener extends _AWCGBasicServerListener
{
    public _ANPPSBasicServerListener(EServerType _serverType, int _serverTypeId)
    {
        super(_serverType.ordinal(), _serverTypeId);
    }

    /******************
     * 处理自定义消息
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealCustomMsg(ByteBuffer _msg)
    {
        NPPSGeneralMsgDispather.getInstance().DealProtocol(this, _msg);
    }

    /******************
     * 处理请求
     *
     * @author alzq.z
     * @time Feb 19, 2013 1:36:05 PM
     */
    @Override
    public void dealRequest(WCGBSRequestCommiter _commiter, ByteBuffer _msg)
    {
        NPPSGeneralRequestDispather.getInstance().DealProtocol(_commiter, _msg);
    }
}
