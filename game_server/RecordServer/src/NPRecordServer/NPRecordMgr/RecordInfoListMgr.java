package NPRecordServer.NPRecordMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPRecordServer.NPRecordServer;
import RCSDB.Bo.NpLoginServerInfoBO;

import java.util.HashMap;
import java.util.List;

/**
 * @description: 玩家登录服务器管理器
 * @author: ricci
 * @date: 2022-06-27 11:51:00
 */
public class RecordInfoListMgr
{
    /**
     * 用户登录记录
     */
    private final HashMap<String, RecordInfoList> _m_accountRecordMap;
    /**
     * 保护 _m_accountRecordMgr 锁
     */
    private final MutexObject _m_mutex;

    //////单例的//////
    private static final RecordInfoListMgr _s_instance = new RecordInfoListMgr();

    public static RecordInfoListMgr getInstance()
    {
        return _s_instance;
    }

    private RecordInfoListMgr()
    {
        _m_accountRecordMap = new HashMap<>();
        _m_mutex = new MutexObject();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 查询用户登录角色列表
     * @param _accountId 用户id
     * @return RecordInfoList
     */
    public RecordInfoList lookup(String _accountId)
    {
        _lock();
        try
        {
            return _m_accountRecordMap.get(_accountId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保生成用户登录信息列表
     * @param _accountId 用户id
     * @return RecordInfoList
     */
    public RecordInfoList ensure(String _accountId)
    {
        _lock();
        try
        {
            RecordInfoList recordInfoList = _m_accountRecordMap.get(_accountId);
            if (recordInfoList == null)
            {
                recordInfoList = new RecordInfoList(_accountId, this);
                _m_accountRecordMap.put(_accountId, recordInfoList);
            }
            return recordInfoList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新登录数据
     * @param _accountId    用户id
     * @param _cid          角色id
     * @param _serverTypeId 服务器id
     */
    public void updateLoginRecord(String _accountId, long _cid, int _serverLogicId, int _serverTypeId)
    {
        _lock();
        try
        {
            RecordInfoList recordList = ensure(_accountId);

            RecordInfo info = recordList.ensure(_cid, _accountId, _serverLogicId);

            BM bmObj = NPRecordServer.getInstance().getBM();

            info.getBo().setLastLoginServerId(bmObj, _serverLogicId);
            info.getBo().setAccountId(bmObj, String.valueOf(_accountId));
            info.getBo().setLastLoginTimeMs(bmObj, CommonFunc.getNowTimeMS());
            info.getBo().saveAllMarked(bmObj);

        } finally
        {
            _unlock();
        }
    }

    public boolean initFormDB()
    {
        List<NpLoginServerInfoBO> boList = NPRecordServer.getInstance().getBM().getBM(NpLoginServerInfoBO.class).s_findAll();
        for (NpLoginServerInfoBO bo : boList)
        {
            if (bo == null)
            {
                continue;
            }
            RecordInfoList recordInfoList = ensure(bo.getAccountId());
            recordInfoList.initFormDB(bo);
        }
        return true;
    }
}
