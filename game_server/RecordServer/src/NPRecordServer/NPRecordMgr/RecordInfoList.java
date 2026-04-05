package NPRecordServer.NPRecordMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPRecordServer.NPRecordServer;
import RCSDB.Bo.NpLoginServerInfoBO;

import java.util.ArrayList;

/**
 * @description: 玩家登录服务器列表
 * @author: ricci
 * @date: 2022-06-27 11:50:45
 */
public class RecordInfoList
{
    /**
     * 用户id
     */
    private final String _m_accountId;

    /**
     * 用户登录过的所有服务器列表
     */
    private final ArrayList<RecordInfo> _m_recordInfoList;

    /**
     * 隶属管理器
     */
    private final RecordInfoListMgr _m_recordMgr;

    /**
     * _m_recordInfoList 列表锁对象
     */
    private final MutexAtom _m_mutex;

    public RecordInfoList(String _accountId, RecordInfoListMgr _mgr)
    {
        this._m_accountId = _accountId;
        this._m_recordInfoList = new ArrayList<>();
        _m_recordMgr = _mgr;
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }
    //region get && set


    public RecordInfoListMgr getRecordMgr()
    {
        return _m_recordMgr;
    }

    public String getAccountId()
    {
        return _m_accountId;
    }

    public ArrayList<RecordInfo> getRecordInfoList()
    {
        return new ArrayList<>(_m_recordInfoList);
    }

    //endregion

    /**
     * 根据 cid 查询角色
     * @param _cid 角色id
     * @return RecordInfo 登录记录
     */
    public RecordInfo lookup(long _cid)
    {
        _lock();
        try
        {
            for (RecordInfo recordInfo : _m_recordInfoList)
            {
                if (recordInfo == null)
                {
                    continue;
                }
                if (recordInfo.getCid() == _cid)
                {
                    return recordInfo;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保创建登录记录
     * @param cid           角色id
     * @param _accountId    用户id
     * @param _serverTypeId 服务器类型id
     * @return RecordInfo 登录记录
     */
    public RecordInfo ensure(long cid, String _accountId, int _serverTypeId)
    {
        _lock();
        try
        {
            RecordInfo recordInfo = lookup(cid);
            if (recordInfo == null)
            {
                BM bmObj = NPRecordServer.getInstance().getBM();

                NpLoginServerInfoBO bo = new NpLoginServerInfoBO();
                bo.setAccountId(bmObj, _accountId);
                bo.setCid(bmObj, cid);
                bo.setLastLoginServerId(bmObj, _serverTypeId);
                bo.setLastLoginTimeMs(bmObj, CommonFunc.getNowTimeMS());
                bo.insert(bmObj);

                recordInfo = new RecordInfo(this, bo);
                _m_recordInfoList.add(recordInfo);
            }
            return recordInfo;
        } finally
        {
            _unlock();
        }
    }

    public ArrayList<NP_SYS_PlayerJoinedUSInfo> makeProto()
    {
        _lock();
        try
        {
            ArrayList<NP_SYS_PlayerJoinedUSInfo> list = new ArrayList<>();

            for (RecordInfo recordInfo : _m_recordInfoList)
            {
                if (recordInfo == null)
                {
                    continue;
                }
                list.add(recordInfo.makeProto());
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (RecordInfo recordInfo : _m_recordInfoList)
        {
            sb.append("\n").append(recordInfo.toString()).append("\n");
        }
        return "RecordInfoList{" +
                "_m_accountId=" + _m_accountId +
                ", _m_recordInfoList=" + sb +
                ", _m_recordMgr=" + _m_recordMgr +
                ", _m_mutex=" + _m_mutex +
                '}';
    }

    /**
     * 从bo初始化
     * @param _bo
     */
    public void initFormDB(NpLoginServerInfoBO _bo)
    {
        _lock();
        try
        {
            _m_recordInfoList.add(new RecordInfo(this, _bo));
        } finally
        {
            _unlock();
        }
    }
}
