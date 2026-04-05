package NPLoginServer.NPLSQueueMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPLoginServer.NPGCListener.NPLSGCListener;
import NPLoginServer.NPLSQueueMgr.SynTask.NPSynTaskRequestEnterCode;

import java.util.LinkedList;

/****************
 * 登录服务器等待队列管理器
 * @author Administrator
 *
 */
public class NPLSQueueMgr
{
    private static NPLSQueueMgr _g_instance = new NPLSQueueMgr();

    public static NPLSQueueMgr getInstance()
    {
        return _g_instance;
    }

    private long _m_lCurIndex;
    private LinkedList<NPLSQueueInfo> _m_lWaitingQueue;

    private MutexAtom _m_mutex;

    protected NPLSQueueMgr()
    {
        _m_lCurIndex = 1;
        _m_lWaitingQueue = new LinkedList<NPLSQueueInfo>();

        _m_mutex = new MutexAtom();

        //开启任务定时进行队列处理
        ALSynTaskManager.getInstance().regTask(new NPSynTaskRequestEnterCode(), 500);
    }

    /**
     * 获取当前的最大索引数据
     */
    public long getCurIndex()
    {
        return _m_lCurIndex;
    }

    /**************
     * 尝试进入队列，并返回新的队列节点
     * @return
     */
    public NPLSQueueInfo enterQueue(NPLSGCListener _listener)
    {
        _lock();

        try
        {
            NPLSQueueInfo queueInfo = new NPLSQueueInfo(_listener, _m_lCurIndex);
            //累加序号
            _m_lCurIndex++;

            //加入队列
            _m_lWaitingQueue.add(queueInfo);

            return queueInfo;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 将节点放回队列处理
     * @param _listener
     * @return
     */
    public void pushBackQueue(NPLSQueueInfo _queueInfo)
    {
        _lock();

        try
        {
            //加入队列
            _m_lWaitingQueue.addFirst(_queueInfo);
        } finally
        {
            _unlock();
        }
    }

    /**************
     * 获取第一个节点对象数据
     * @return
     */

    public NPLSQueueInfo getFirstQueue()
    {
        _lock();

        try
        {
            NPLSQueueInfo queueInfo;

            do
            {
                if (_m_lWaitingQueue.isEmpty())
                    return null;

                queueInfo = _m_lWaitingQueue.getFirst();

                if (null != queueInfo && queueInfo.enable())
                    return queueInfo;

                //无效则删除数据
                _m_lWaitingQueue.removeFirst();
            } while (!_m_lWaitingQueue.isEmpty());

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**************
     * 移除第一个节点对象数据
     * @return
     */
    public NPLSQueueInfo removeFirstQueue()
    {
        _lock();

        try
        {
            return _m_lWaitingQueue.removeFirst();
        } finally
        {
            _unlock();
        }
    }

    /**************
     * 判断队列是否为空
     * @return
     */
    public boolean isEmpty()
    {
        _lock();

        try
        {
            return _m_lWaitingQueue.isEmpty();
        } finally
        {
            _unlock();
        }
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }
}
