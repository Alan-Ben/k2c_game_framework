package CrossDataServer.CrossDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;

import java.util.Hashtable;

/**
 * 全局的数据管理器，根据类型注册不同的实际管理器对象
 */
@SuppressWarnings("rawtypes")
public class CrossDataCore
{
	private static CrossDataCore _g_instance = new CrossDataCore();
    public static CrossDataCore getInstance()
    {
        if(null == _g_instance)
            _g_instance = new CrossDataCore();
        return _g_instance;
    }

    //保存不同管理器的表
    private Hashtable<Integer, _ACrossDataTypeMgr> _m_htCrossDataTable;
    private MutexAtom _m_mutex;

    protected CrossDataCore()
    {
        _m_htCrossDataTable = new Hashtable<Integer, _ACrossDataTypeMgr>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /**
     * 获取对应的管理器对象
     * @param _type
     * @return
     */
    public _ACrossDataTypeMgr getCrossDataMgr(int _type)
    {
        _lock();

        try
        {
            return _m_htCrossDataTable.get(_type);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 注册数据管理器
     * @param _dataMgr
     */
    public void regCrossDataMgr(_ACrossDataTypeMgr _dataMgr)
    {
        if(null == _dataMgr)
            return ;

        _lock();

        try
        {
            if(_m_htCrossDataTable.contains(_dataMgr.getDataType()))
            {
                ALServerLog.Sys("Cross Data Mgr reg multi times! for dataType: " + _dataMgr.getDataType());
                return ;
            }

            //放入数据集
            _m_htCrossDataTable.put(_dataMgr.getDataType(), _dataMgr);
        }
        finally {
            _unlock();
        }
    }
}
