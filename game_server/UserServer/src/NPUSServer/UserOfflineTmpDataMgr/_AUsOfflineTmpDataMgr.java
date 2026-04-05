package NPUSServer.UserOfflineTmpDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import NPCommon.Util.CallBack._ICallBackResultT;

import java.util.Hashtable;

/**
 * Us服务器上针对某种离线数据进行临时管理的管理器
 */
public abstract class _AUsOfflineTmpDataMgr<T extends  _IUserOfflineTmpDataInfo, DT extends _ATUserOfflineTmpDataMgr<T>>
{
    private UsOfflineTmpDataCore _m_dcDataCore;
    private int _m_iDataType;

    //根据Cid划分不同数据管理器
    private Hashtable<Long, DT> _m_htCidTmpDataMgr;

    private MutexAtom _m_mutex;

    protected _AUsOfflineTmpDataMgr(UsOfflineTmpDataCore _dataCore, int _dataType)
    {
        _m_dcDataCore = _dataCore;
        _m_iDataType = _dataType;

        _m_htCidTmpDataMgr = new Hashtable<Long, DT>();

        _m_mutex = new MutexAtom();
    }

    public UsOfflineTmpDataCore getDataCore() {return _m_dcDataCore;}
    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    public int getDataType() {return _m_iDataType;}

    /**
     * 移除角色临时数据管理器，一般在角色上线的时候进行处理
     * @param _cid
     */
    public void rmvCidDataMgr(long _cid)
    {
        _lock();

        try
        {
            _m_htCidTmpDataMgr.remove(_cid);
        }
        finally {
            _unlock();
        }
    }

    /***
     * 获取对应玩家的对应数据，并在回调中进行处理
     * @param _cid
     * @param _dataId
     * @param _callback
     */
    public void localDoOfflineData(long _cid, long _dataId, _ICallBackResultT _callback)
    {
        _lock();

        try {
            DT cidDataMgr = _ensureCidTmpDataMgr(_cid);
            if (null == cidDataMgr) {
                ALServerLog.Error("ensure cid TmpDataMgr failed for cid: " + _cid);
                return;
            }

            //查询并处理
            cidDataMgr._comfirmData_InLock(new UserOfflineTmpDataLocalDealer(_callback), _dataId);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 根据Cid获取对应的离线数据管理器
     * @param _cid
     * @return
     */
    protected DT _ensureCidTmpDataMgr(long _cid)
    {
        _lock();

        try
        {
            DT dataMgr = _m_htCidTmpDataMgr.get(_cid);
            if(null != dataMgr)
                return dataMgr;

            //创建并放入数据集
            dataMgr = _createCidTmpDataMgr(_cid);
            _m_htCidTmpDataMgr.put(_cid, dataMgr);

            return dataMgr;
        }
        finally {
            _unlock();
        }
    }

    /**
     * 创建一个对应的Cid数据管理对象
     * @param _cid
     * @return
     */
    protected abstract DT _createCidTmpDataMgr(long _cid);
}
