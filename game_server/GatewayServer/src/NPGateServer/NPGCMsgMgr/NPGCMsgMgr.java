package NPGateServer.NPGCMsgMgr;

import java.util.Hashtable;
import java.util.concurrent.locks.ReentrantLock;

/***************
 * 客户端连接对象的消息管理器对象
 * @author Administrator
 *
 */
public class NPGCMsgMgr
{
    private static NPGCMsgMgr _g_instance = new NPGCMsgMgr();

    public static NPGCMsgMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = new NPGCMsgMgr();

        return _g_instance;
    }

    //处理对象的序列号生成器
    private int _m_iDealerSerializeMaker;
    //客户端消息处理对象映射表，在客户端掉线的时候不会马上处理
    private Hashtable<String, NPGCMsgDealer> _m_htGCMsgDealerTable;
    private Hashtable<Integer, NPGCMsgDealer> _m_htGCMsgDealerIdTable;

    //锁对象
    private ReentrantLock _m_mutex;

    protected NPGCMsgMgr()
    {
        _m_iDealerSerializeMaker = 1;
        _m_htGCMsgDealerTable = new Hashtable<String, NPGCMsgDealer>();
        _m_htGCMsgDealerIdTable = new Hashtable<Integer, NPGCMsgDealer>();

        _m_mutex = new ReentrantLock();
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
     * 获取对应的消息处理对象
     */
    public NPGCMsgDealer tryGetMsgDealer(String _uid)
    {
        _lock();

        try
        {
            NPGCMsgDealer dealer = _m_htGCMsgDealerTable.get(_uid);

            if (null == dealer)
            {
                _m_iDealerSerializeMaker++;
                dealer = new NPGCMsgDealer(_uid, _m_iDealerSerializeMaker);
                _m_htGCMsgDealerTable.put(_uid, dealer);
                _m_htGCMsgDealerIdTable.put(dealer.getSerialize(), dealer);
            }

            return dealer;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取对应序列号的消息处理对象
     */
    public NPGCMsgDealer tryGetMsgDealerBySerialize(int _serialize)
    {
        _lock();

        try
        {
            return _m_htGCMsgDealerIdTable.get(_serialize);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 删除对应的用户消息处理对象
     */
    public void removeMsgDealer(NPGCMsgDealer _msgDealer, long _gcSessionId)
    {
        if (null == _msgDealer)
            return;

        //对应的客户端处理对象不同则不处理
        if (_msgDealer.getGCSessionId() != _gcSessionId)
            return;

        _lock();

        try
        {
            //清除序列号数据队列
            _m_htGCMsgDealerIdTable.remove(_msgDealer.getSerialize());
            NPGCMsgDealer dealer = _m_htGCMsgDealerTable.remove(_msgDealer.getUid());
            //判断删除数据是否出错，如出错则放回
            if (_msgDealer != dealer)
            {
                if (null != dealer)
                    _m_htGCMsgDealerTable.put(_msgDealer.getUid(), dealer);
                return;
            }
        } finally
        {
            _unlock();

            //释放数据
            _msgDealer.dispose();
        }
    }

    /**
     * 删除对应的用户处理对象
     */
    public NPGCMsgDealer removeMsgDealer(String _uid)
    {
        _lock();

        try
        {
            NPGCMsgDealer dealer = _m_htGCMsgDealerTable.remove(_uid);
            if (null != dealer)
                _m_htGCMsgDealerIdTable.remove(dealer.getSerialize());

            return dealer;
        } finally
        {
            _unlock();
        }
    }
}
