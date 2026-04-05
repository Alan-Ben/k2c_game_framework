package NPUSServer.UserOfflineTmpDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPUSServer.NPUserServer;

import java.util.ArrayList;

/**
 * 服务器内离线数据管理中心
 */
public class UsOfflineTmpDataCore {
    private NPUserServer _m_server;
    //不同类型的数据管理器
    private ArrayList<_AUsOfflineTmpDataMgr> _m_lDataMgrList;

    private UsOfflineDataFunc _m_dfDataFunc;

    private MutexAtom _m_mutex;

    public UsOfflineTmpDataCore(NPUserServer _server)
    {
        _m_server = _server;
        _m_lDataMgrList = new ArrayList<_AUsOfflineTmpDataMgr>();
        _m_mutex = new MutexAtom();

        _m_dfDataFunc = new UsOfflineDataFunc(this);
    }

    public NPUserServer getUSServer() {return _m_server;}
    public UsOfflineDataFunc getDataFunc() {return _m_dfDataFunc;}
    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /**
     * 注册一个数据管理器
     * @param _dataMgr
     */
    public void initRegUsOfflineTmpDataMgr_Unsafe(_AUsOfflineTmpDataMgr _dataMgr)
    {
        if(null == _dataMgr)
            return ;

        if (null != _lookupDataMgr(_dataMgr.getDataType())) {
            ALServerLog.Error("multi reg UsOfflineTmpDataMgr for dataType: " + _dataMgr.getDataType());
            return;
        }

        //每次创建新的，这样旧的数据列表还是可用的
        _m_lDataMgrList = new ArrayList<_AUsOfflineTmpDataMgr>(_m_lDataMgrList);
        _m_lDataMgrList.add(_dataMgr);
    }
    public void regUsOfflineTmpDataMgr(_AUsOfflineTmpDataMgr _dataMgr)
    {
        if(null == _dataMgr)
            return ;

        _lock();

        try {
            if (null != _lookupDataMgr(_dataMgr.getDataType())) {
                ALServerLog.Error("multi reg UsOfflineTmpDataMgr for dataType: " + _dataMgr.getDataType());
                return;
            }

            //每次创建新的，这样旧的数据列表还是可用的
            _m_lDataMgrList = new ArrayList<_AUsOfflineTmpDataMgr>(_m_lDataMgrList);
            _m_lDataMgrList.add(_dataMgr);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 清理某个角色在所有数据集中的数据
     * @param _cid
     */
    public void clearCidData(long _cid)
    {
        ArrayList<_AUsOfflineTmpDataMgr> tmpList;
        _lock();

        try
        {
            //获取队列
            tmpList = _m_lDataMgrList;
        }
        finally {
            _unlock();
        }

        //每个管理器中删除对应Cid数据
        for(_AUsOfflineTmpDataMgr dataMgr : tmpList)
        {
            dataMgr.rmvCidDataMgr(_cid);
        }
    }

    /***
     * 获取对应玩家的对应数据，并在回调中进行处理
     * @param _dataType
     * @param _cid
     * @param _dataId
     * @param _callback
     * @param <T>
     */
    public <T extends  _IUserOfflineTmpDataInfo> void localDoOfflineData(int _dataType, long _cid, long _dataId, _ICallBackResultT<T> _callback)
    {
        _AUsOfflineTmpDataMgr dataMgr = _lookupDataMgr(_dataType);
        if(null == dataMgr)
        {
            ALServerLog.Error("lookup UsOfflineTmpDataMgr failed for dataType: " + _dataType);
            //调用错误回调
            if(null != _callback)
                _callback.onRunOver(CommErr.OBJ_ERR, null);
            return ;
        }

        //调用对应的数据管理器进行处理
        dataMgr.localDoOfflineData(_cid, _dataId, _callback);
    }

    /**
     * 根据数据类型获取对应的数据管理器
     * @param _dataType
     * @return
     */
    protected _AUsOfflineTmpDataMgr _lookupDataMgr(int _dataType)
    {
        _lock();

        try
        {
            for (_AUsOfflineTmpDataMgr dataMgr : _m_lDataMgrList)
            {
                if (dataMgr.getDataType() == _dataType)
                {
                    return dataMgr;
                }
            }

            return null;
        }
        finally {
            _unlock();
        }
    }
}
