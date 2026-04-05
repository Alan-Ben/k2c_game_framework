package NPPlatServer.NPPlatUserMgr;

import NPPlatServer.NPPS_Listener.PS_GSListener;

/****************
 *  单个用户的处理信息存储对象
 * @author Administrator
 *
 */
public class NPPlatUserHandleInfo
{
    /**
     * 用户Id
     */
    private long _m_lUid;
    /**
     * 用户连接的连接服务器对象
     */
    private PS_GSListener _m_gsListener;

    public NPPlatUserHandleInfo(long _uid, PS_GSListener _gsListener)
    {
        _m_lUid = _uid;
        _m_gsListener = _gsListener;
    }

    public long getUid()
    {
        return _m_lUid;
    }

    public PS_GSListener getGSListener()
    {
        return _m_gsListener;
    }
}
