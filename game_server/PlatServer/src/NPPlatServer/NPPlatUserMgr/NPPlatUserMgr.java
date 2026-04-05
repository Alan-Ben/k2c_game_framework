package NPPlatServer.NPPlatUserMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPPlatServer.NPPS_Listener.PS_GSListener;
import NPPlatServer.NPPS_Listener.PS_USListener;

import java.util.HashMap;

/**************
 * 游戏中的所有用户管理对象
 * @author Administrator
 *
 */
public class NPPlatUserMgr
{
    private static NPPlatUserMgr _g_instance = new NPPlatUserMgr();

    public static NPPlatUserMgr getInstance()
    {
        return _g_instance;
    }

    /**
     * 用户处理对象的映射表
     */
    private HashMap<Long, NPPlatUserHandleInfo> _m_hmUserHandleInfo;
    private MutexAtom _m_handleMutex;

    protected NPPlatUserMgr()
    {
        _m_hmUserHandleInfo = new HashMap<Long, NPPlatUserHandleInfo>();
        _m_handleMutex = new MutexAtom();
    }

    /**************
     * 注册一个用户的处理信息
     * @param _uid
     * @param _gsListener
     */
    public void regUserHandleInfo(long _uid, PS_GSListener _gsListener)
    {
        if (null == _gsListener)
            return;

        _lockHandle();

        try
        {
            NPPlatUserHandleInfo handleInfo = new NPPlatUserHandleInfo(_uid, _gsListener);
            _m_hmUserHandleInfo.put(_uid, handleInfo);
        } finally
        {
            _unlockHandle();
        }
    }

    /**************
     * 取出当前用户的处理对象
     * @param _uid
     * @param _gsListener
     */
    public NPPlatUserHandleInfo popUserHandleInfo(long _uid)
    {
        _lockHandle();

        try
        {
            return _m_hmUserHandleInfo.remove(_uid);
        } finally
        {
            _unlockHandle();
        }
    }

    protected void _lockHandle()
    {
        _m_handleMutex.lock();
    }

    protected void _unlockHandle()
    {
        _m_handleMutex.unlock();
    }

    public void transferPlayerDataFromUs2Us(PS_USListener oldUsListener, PS_USListener newUsListner, long uid)
    {
//		WCGRpcSender oldUsRpc= new WCGRpcSender(oldUsListener);
//		WCGRpcSender newUsRpc= new WCGRpcSender(newUsListner);
        //oldUsRpc.request(_rpcData, _callback);
    }
}
