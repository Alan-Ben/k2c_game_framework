package NPHttpServer.NPWhiteAccMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import HSDB.Bo.HsWhiteAccBO;
import NP2CS_RB.p002_ServerInfoOP.NP2CS_RB_002_015_RetUpdateWhiteAccList;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPEnum.EWhiteAccOpType;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

/****************
 * 白名单管理
 */
public class NPWhiteAccMgr
{
    private static NPWhiteAccMgr _g_instance = new NPWhiteAccMgr();

    public static NPWhiteAccMgr getInstance()
    {
        return _g_instance;
    }

    //白名单账号数据容器
    private HashSet<String> _m_hsWhiteAccountSet;
    //锁对象
    private MutexAtom _m_mutex;

    public NPWhiteAccMgr()
    {
        _m_hsWhiteAccountSet = new HashSet<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public boolean initFromDB()
    {
        List<HsWhiteAccBO> boList = NPHttpServer.getInstance().getBM().getBM(HsWhiteAccBO.class).s_findAll();
        if (null == boList)
        {
            CommLog.error("NPWhiteAccMgr.initFromDB fail");
            return false;
        }

        for (HsWhiteAccBO bo : boList)
        {
            if (!initFromDb(bo.getAcc()))
            {
                CommLog.error("NPWhiteAccMgr initFromDB has repeat bo dbId:{} acc:{}", bo.getId(), bo.getAcc());
            }
        }

        return true;
    }

    /**
     * 白名单初始化
     * @param _acc
     * @return
     */
    public boolean initFromDb(String _acc)
    {
        _lock();
        try
        {
            if (_m_hsWhiteAccountSet.contains(_acc))
                return false;

            _m_hsWhiteAccountSet.add(_acc);

            return true;
        } finally
        {
            _unlock();
        }
    }

    public boolean addAcc(String _acc)
    {
        _lock();
        try
        {
            if (_m_hsWhiteAccountSet.contains(_acc))
                return true;

            BM bmObj = NPHttpServer.getInstance().getBM();
            HsWhiteAccBO bo = new HsWhiteAccBO();
            bo.setAcc(bmObj, _acc);
            bo.setCreateAt(bmObj, CommonFunc.getNowTimeMS());
            bo.insert(bmObj);

            _m_hsWhiteAccountSet.add(_acc);

            // 推送添加操作到CommonServer
            notifyCommonServerAdd(_acc);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 指定账号移除白名单
     * @param _acc
     */
    public boolean removeAcc(String _acc)
    {
        _lock();
        try
        {
            boolean isSuc = _m_hsWhiteAccountSet.remove(_acc);
            if (isSuc)
            {
                NPHttpServer.getInstance().getBM().getBM(HsWhiteAccBO.class).delAll("acc", _acc);

                // 推送删除操作到CommonServer
                notifyCommonServerRemove(_acc);
            }
            return isSuc;
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 检测指定账户是否白名单
     * @param _acc
     * @return
     */
    public boolean checkUidInWhiteList(String _acc)
    {
        _lock();
        try
        {
            return _m_hsWhiteAccountSet.contains(_acc);
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 打印所有白名单列表
     */
    @Override
    public String toString()
    {
        _lock();
        try
        {
            return CommonFunc.list2String(_m_hsWhiteAccountSet, ',');
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有白名单账号列表
     *
     * @return 白名单账号列表
     */
    public List<String> getAllAccList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_hsWhiteAccountSet);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 推送添加账号到CommonServer
     *
     * @param _acc 账号
     *
     * 执行流程：
     * 1. 构造增量更新协议（ADD操作）
     * 2. 向CommonServer发送NP2CS_R_002_015请求
     *
     * 注意：此方法必须在锁内调用
     */
    private void notifyCommonServerAdd(String _acc)
    {
        NPHttpServer.getInstance().sendRequestToBSServer(
            EServerType.SINGLE.ordinal(),
            ENPSingleServerType.COMMON.ordinal(),
            NP2CS_R_Writer_002_ServerInfoOp.make_015_ReqUpdateWhiteAccList(EWhiteAccOpType.ADD, _acc),
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2CS_RB_002_015_RetUpdateWhiteAccList();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retProto)
                {
                    CommLog.info("Notify CommonServer add white acc success, acc: {}", _acc);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    CommLog.error("Notify CommonServer add white acc fail, acc: {}, errCode: {}", _acc, _errCode);
                }
            });
    }

    /**
     * 推送删除账号到CommonServer
     *
     * @param _acc 账号
     *
     * 执行流程：
     * 1. 构造增量更新协议（REMOVE操作）
     * 2. 向CommonServer发送NP2CS_R_002_015请求
     *
     * 注意：此方法必须在锁内调用
     */
    private void notifyCommonServerRemove(String _acc)
    {
        NPHttpServer.getInstance().sendRequestToBSServer(
            EServerType.SINGLE.ordinal(),
            ENPSingleServerType.COMMON.ordinal(),
            NP2CS_R_Writer_002_ServerInfoOp.make_015_ReqUpdateWhiteAccList(EWhiteAccOpType.REMOVE, _acc),
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2CS_RB_002_015_RetUpdateWhiteAccList();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retProto)
                {
                    CommLog.info("Notify CommonServer remove white acc success, acc: {}", _acc);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    CommLog.error("Notify CommonServer remove white acc fail, acc: {}, errCode: {}", _acc, _errCode);
                }
            });
    }
}
