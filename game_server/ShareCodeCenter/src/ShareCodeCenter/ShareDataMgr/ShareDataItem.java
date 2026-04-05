package ShareCodeCenter.ShareDataMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBConditionObj;
import NPCommon.CommonCache.ELoadState;
import NPCommon.CommonCache._ITimeKeyData;
import NPCommon.DB.BaseBO;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.Delegate.ADelegateTwo;
import NPCommon.Util.Delegate.HandlerTwo;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class ShareDataItem<T extends BaseBO & _ICreateTableSql> implements _ITimeKeyData<Long>
{
    private _AShareDataMgr<?> _m_mgr;
    private long _m_serial;

    private volatile ELoadState _m_loadState = ELoadState.eNotLoad;
    private volatile _IALProtocolStructure _m_cacheData;
    private ADelegateTwo<Boolean, _IALProtocolStructure> OnLoadOver = new ADelegateTwo<>(this);
    private int _m_timeStamp;
    private final MutexAtom _m_locker = new MutexAtom();

    public ShareDataItem(_AShareDataMgr<?> _mgr, long _serial)
    {
        _m_mgr = _mgr;
        _m_serial = _serial;
    }

    protected void _lock()
    {
        _m_locker.lock();
    }

    protected void _unlock()
    {
        _m_locker.unlock();
    }

    public ELoadState getLoadState()
    {
        return _m_loadState;
    }

    public void setLoadState(ELoadState _loadState)
    {
        _m_loadState = _loadState;
    }

    /******
     * 异步加载,加锁
     * @param _handler
     */
    public void asyncLoad(HandlerTwo<Boolean, _IALProtocolStructure> _handler)
    {
        _lock();
        try
        {
            boolean bHandlerDealed = false;
            if (_m_cacheData != null)
            {//如果数据不为空
                bHandlerDealed = true;//设置回调已经处理了
                if (null != _handler) //直接回调给调用方
                {
                    ALSynTaskManager.getInstance().regTask(() ->
                    {
                        _handler.handle(true, _m_cacheData);
                    });
                }
            }

            if (_m_cacheData == null)//过期或未加载，需要加载
            {
                if (null != _handler && !bHandlerDealed)
                {//调用方回调未处理，先加入回调列表
                    OnLoadOver.addHandler(null, _handler);
                }
                if (getLoadState() != ELoadState.eLoading)
                {//加载还未开始,开始加载

                    setLoadState(ELoadState.eLoading);
                    _asyncLoad(_result ->
                    {
                        _lock();
                        try
                        {
                            if (!_result.isSucc())
                            {
                                setLoadState(ELoadState.eNotLoad);
                                _m_cacheData = null;
                                OnLoadOver.onAsyncEvent(false, null);
                            } else
                            {
                                setLoadState(ELoadState.eLoaded);
                                _m_cacheData = _result.getData();
                                OnLoadOver.onAsyncEvent(true, _result.getData());
                            }
                        } finally
                        {
                            _unlock();
                        }
                    });
                }
            }
        } finally
        {
            _unlock();
        }
    }

    private void _asyncLoad(_ICallBackT<ResultOne<_IALProtocolStructure>> _callback)
    {
        //计算分表的索引
        int tableIndex = _m_mgr.getTableIndex(_m_serial);

        ALMySqlDBConditionObj objCond = new ALMySqlDBConditionObj();
        objCond.setTablesName(_m_mgr.getTableName());
        objCond.addAndEquals(_m_mgr.getTableSerialFieldName(), _m_serial);

        //查询数据
        ALAsynTaskManager.getInstance().regTask(_m_mgr.getDBTaskThreadIndex(), () ->
        {
            @SuppressWarnings("unchecked")
            ArrayList<T> list = (ArrayList<T>) _m_mgr.getDBObj().getListByCondition(objCond, _m_mgr.ensureBaseDBBO(tableIndex));
            if (list == null || list.isEmpty())
            {
                _callback.onRunOver(new ResultOne<>(CommErr.SYS_ERR, null));
                return;
            }

            //对应的数据bo
            T bo = list.get(0);
            //获取bo上的数据数组
            byte[] dateByteList = _m_mgr._getDataInBo(bo);
            if (dateByteList == null)
            {
                _callback.onRunOver(new ResultOne<>(CommErr.SYS_ERR, null));
                return;
            }

            //创建数据实例，读取数据
            _IALProtocolStructure dataInstance = _m_mgr.createDataInstance();
            try
            {
                dataInstance.readPackage(ByteBuffer.wrap(dateByteList));
            } catch (Exception e)
            {
                CommLog.error("_AShareDataMgr loadData readPackage error", e);
                _callback.onRunOver(new ResultOne<>(CommErr.SYS_ERR, null));
                return;
            }

            _callback.onRunOver(new ResultOne<>(Result.SUCC, dataInstance));
        });
    }

    @Override
    public Long getkey()
    {
        return _m_serial;
    }

    @Override
    public int getTimeStamp()
    {
        return _m_timeStamp;
    }

    @Override
    public void setTimeStamp(int _timeStamp)
    {
        _m_timeStamp = _timeStamp;
    }

    @Override
    public void callbackOnRemoved()
    {

    }
}
