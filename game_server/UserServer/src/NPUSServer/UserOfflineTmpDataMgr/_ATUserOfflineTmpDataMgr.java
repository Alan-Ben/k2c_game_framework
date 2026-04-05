package NPUSServer.UserOfflineTmpDataMgr;

import ALBasicServer.ALTask._IALAsynCallBackTask;
import NPCommon.ErrMain.CommErr;
import NPUSServer.NPUserServer;

import java.util.ArrayList;
import java.util.Hashtable;

/**
 * 玩家离线状态下，临时数据加载管理器
 */
public abstract class _ATUserOfflineTmpDataMgr<T extends  _IUserOfflineTmpDataInfo>
{
    private _AUsOfflineTmpDataMgr _m_dataMgr;
    private long _m_lCid;

    //角色下的临时数据对象管理器
    private ArrayList<_IUserOfflineTmpDataInfo> _m_lTmpDataList;
    //当数据还在查询过程的时候，存储当前查询数据的数据结构体
    private Hashtable<Long, ArrayList<_IUserOfflineTmpDataSearchDoneDealer>> _m_htDataRequestTable;

    protected _ATUserOfflineTmpDataMgr(_AUsOfflineTmpDataMgr _dataMgr, long _cid)
    {
        _m_dataMgr = _dataMgr;
        _m_lCid = _cid;
        _m_htDataRequestTable = new Hashtable<>();
        _m_lTmpDataList = new ArrayList<_IUserOfflineTmpDataInfo>();
    }

    public NPUserServer getUSServer() {return _m_dataMgr.getDataCore().getUSServer();}
    public long getCid() {return _m_lCid;}

    /**
     * 确认数据并返回，带入请求的协议方便判断可能不同的数据访问类型
     */
    protected void _comfirmData_InLock(_IUserOfflineTmpDataSearchDoneDealer _searchDoneDealer, long _dataId)
    {
        //先查询数据，如果有直接返回，如果无则调用数据对象重新查询处理
        _IUserOfflineTmpDataInfo tmpData = _lookupTmpData_InLock(_dataId);
        if(null != tmpData)
        {
            if(null != _searchDoneDealer)
                _searchDoneDealer.onSearchDone_InLock(tmpData);
            return ;
        }

        //查询是否有数据在加载过程中
        ArrayList<_IUserOfflineTmpDataSearchDoneDealer> requestList = _m_htDataRequestTable.get(_dataId);
        if(null == requestList)
        {
            requestList = new ArrayList<_IUserOfflineTmpDataSearchDoneDealer>();
            _m_htDataRequestTable.put(_dataId, requestList);
        }

        //加入队列
        requestList.add(_searchDoneDealer);

        //调用数据查询并返回的处理
        _loadDataOp(_dataId, new _IALAsynCallBackTask<T>() {
            @Override
            public void dealSuc(T _dataInfo) {
                _m_dataMgr._lock();

                try {
                    if (null == _dataInfo) {
                        //直接创建通用错误对象
                        _IUserOfflineTmpDataInfo errDataInfo = new UserOfflineTmpDataInfo_ComErr(_dataId, CommErr.OBJ_ERR);
                        //放入数据集
                        _m_lTmpDataList.add(errDataInfo);

                        //调用处理函数
                        _dealDataCallback_InLock(errDataInfo);
                        return;
                    }

                    //将数据放入通用数据集
                    _m_lTmpDataList.add(_dataInfo);

                    //调用处理函数
                    _dealDataCallback_InLock(_dataInfo);
                }
                finally {
                    _m_dataMgr._unlock();
                }
            }

            @Override
            public void dealFail() {
                _m_dataMgr._lock();

                try {
                    //直接创建通用错误对象
                    _IUserOfflineTmpDataInfo dataInfo = new UserOfflineTmpDataInfo_ComErr(_dataId, CommErr.OBJ_ERR);
                    //放入数据集
                    _m_lTmpDataList.add(dataInfo);

                    //调用处理函数
                    _dealDataCallback_InLock(dataInfo);
                }
                finally {
                    _m_dataMgr._unlock();
                }
            }
        });
    }

    /**
     * 处理缓存的回调处理对象
     * @param _dataInfo
     */
    protected void _dealDataCallback_InLock(_IUserOfflineTmpDataInfo _dataInfo)
    {
        if(null == _dataInfo)
            return ;

        //查询数据
        ArrayList<_IUserOfflineTmpDataSearchDoneDealer> requestList = _m_htDataRequestTable.remove(_dataInfo.getDataId());
        if(null == requestList)
            return ;

        //逐个处理
        for(_IUserOfflineTmpDataSearchDoneDealer requestInfo : requestList)
        {
            requestInfo.onSearchDone_InLock(_dataInfo);
        }
    }

    /**
     * 通过Id检索数据
     * @param _dataId
     * @return
     */
    protected _IUserOfflineTmpDataInfo _lookupTmpData_InLock(long _dataId)
    {
        for(_IUserOfflineTmpDataInfo tmpData : _m_lTmpDataList)
        {
            if(tmpData.getDataId() == _dataId)
                return tmpData;
        }

        return null;
    }

    /**
     * 实际数据查询加载的处理，处理完毕后调用相关回调
     * @param _dataId
     * @param _callbackDealer
     */
    protected abstract void _loadDataOp(long _dataId, _IALAsynCallBackTask<T> _callbackDealer);
}
