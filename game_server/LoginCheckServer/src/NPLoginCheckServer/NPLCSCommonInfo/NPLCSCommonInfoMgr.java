package NPLoginCheckServer.NPLCSCommonInfo;

import ALServerLog.ALServerLog;
import LCSDB.Bo.CommInfoBO;
import NPCommon.DB.BM.BM;
import NPLoginCheckServer.NPLoginCheckServer;

import java.util.List;

/************
 * 通用信息管理对象
 * @author Administrator
 *
 */
public class NPLCSCommonInfoMgr
{
    private static NPLCSCommonInfoMgr _g_instance = new NPLCSCommonInfoMgr();

    public static NPLCSCommonInfoMgr getInstance()
    {
        return _g_instance;
    }

    private boolean _m_bInit;

    /**
     * 下一个用户的Id
     */
    private long _m_lNextUid;
    private CommInfoBO _m_bo;

    protected NPLCSCommonInfoMgr()
    {
        _m_lNextUid = 0;
    }

    /****************
     * 返回是否初始化成功
     * @return
     */
    public boolean init()
    {
        if (_m_bInit)
        {
            ALServerLog.Error("Can not init WCGLCSCommonInfoMgr Multiple time!");
            return true;
        }

        BM bmObj = NPLoginCheckServer.getInstance().getBM();
        List<CommInfoBO> listCommInfo = bmObj.getBM(CommInfoBO.class).s_findAll();
        if (listCommInfo.size() == 0)
        {
            _m_lNextUid = 1000000;
            _m_bo = new CommInfoBO();
            _m_bo.setNextUid(bmObj, _m_lNextUid);
            _m_bo.insert_sync(bmObj);
        } else
        {
            _m_bo = listCommInfo.get(0);
            _m_lNextUid = _m_bo.getNextUid();
        }
        //设置初始化标识位
        _m_bInit = true;
        return true;
    }

    /****************
     * 申请一个新的用户Id
     * @return
     */
    public synchronized long requestNewUid()
    {
        try
        {
            return _m_lNextUid++;
        } finally
        {
            //刷新数据库
            _m_bo.saveNextUid(NPLoginCheckServer.getInstance().getBM(), _m_lNextUid);
        }
    }
}
