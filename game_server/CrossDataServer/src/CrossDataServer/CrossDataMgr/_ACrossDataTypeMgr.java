package CrossDataServer.CrossDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;

import java.util.Hashtable;

/**
 * 全局的数据管理器，根据类型注册不同的实际管理器对象
 */
@SuppressWarnings("rawtypes")
public abstract class _ACrossDataTypeMgr
{
    //数据类型
    private int _m_iDataType;
    //保存不同分组的数据集
    private Hashtable<Long, _ICrossDataMgr> _m_htCrossDataGroupTable;
    private MutexAtom _m_mutex;

    protected _ACrossDataTypeMgr(int _dataType)
    {
        _m_iDataType = _dataType;
        _m_htCrossDataGroupTable = new Hashtable<Long, _ICrossDataMgr>();
        _m_mutex = new MutexAtom();
    }

    public int getDataType() {return _m_iDataType;}

    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /**
     * 获取对应的管理器对象
     * @param _groupId 分组Id
     * @return
     */
    public _ICrossDataMgr ensureCrossDataMgr(long _groupId)
    {
        _lock();

        try
        {
            _ICrossDataMgr dataMgr = _m_htCrossDataGroupTable.get(_groupId);
            if(null == dataMgr)
            {
                dataMgr = _createNewGroupDataMgr(_groupId);
                _m_htCrossDataGroupTable.put(_groupId, dataMgr);
            }

            return dataMgr;
        }
        finally {
            _unlock();
        }
    }

    /**
     * 根据分组Id，创建对应的数据分组管理器对象
     * @param _groupId
     * @return
     */
    protected abstract _ICrossDataMgr _createNewGroupDataMgr(long _groupId);
}
