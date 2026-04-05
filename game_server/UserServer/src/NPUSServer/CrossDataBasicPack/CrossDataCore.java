package NPUSServer.CrossDataBasicPack;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;

import java.util.ArrayList;

/**
 * 如由数据管理器进行了初始化，将在此注册。
 * 并确保如果后续CrossData服务器异常连接后，数据可以正常同步
 */
public class CrossDataCore {
    //所有初始化过的数据管理器队列
    private ArrayList<_ICrossDataMgrInterface> _m_lCrossDataInterfaceList;

    //避免队列管理的锁处理
    private MutexAtom _m_mutex;

    public CrossDataCore()
    {
        _m_lCrossDataInterfaceList = new ArrayList<_ICrossDataMgrInterface>();

        _m_mutex = new MutexAtom();
    }

    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /*********
     * 添加数据管理器
     * @param _dataMgr
     */
    protected void _addCrossDataMgr(_ICrossDataMgrInterface _dataMgr)
    {
        if(null == _dataMgr)
            return ;

        _lock();

        try {
            if (_m_lCrossDataInterfaceList.contains(_dataMgr)) {
                ALServerLog.Error("Multi reg crossDataMgr:" + _dataMgr.getDataType());
                return;
            }

            //添加到数据集
            _m_lCrossDataInterfaceList.add(_dataMgr);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 在跨服数据管理服务器上线的时候触发的处理函数，需要各管理器进行信息同步
     */
    public void onCrossDataServerOnline()
    {
        ArrayList<_ICrossDataMgrInterface> tmpList = new ArrayList<_ICrossDataMgrInterface>();
        _lock();

        try {
            //由于此处理次数不多，这里直接拷贝队列，避免可能的锁冲突
            tmpList.addAll(_m_lCrossDataInterfaceList);
        }
        finally {
            _unlock();
        }

        //逐个服务器进行处理
        for(_ICrossDataMgrInterface dataMgr : tmpList)
        {
            dataMgr.onCrossDataServerOnline();
        }
    }

}
