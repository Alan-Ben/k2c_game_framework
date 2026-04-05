package NPCommon.CommonCache;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Function;

/******
 * 按时间排序的LinkedList，提供的update方法，可以快速检索到对应的节点并进行更新，并移到最后
 * @param <K>
 * @param <T>
 */
public class TimeKeyLinkList<K, T extends _ITimeKeyData<K>>
{
    /*****
     * Link List的节点
     * @param <_DATATYPE>
     */
    private static class LinkNode<_DATATYPE>
    {
        public LinkNode<_DATATYPE> preNode = null;
        public _DATATYPE data;
        public LinkNode<_DATATYPE> nextNode = null;
    }

    private LinkNode<T> _m_firstNode = null;
    private LinkNode<T> _m_lastNode = null;
    private Map<K, LinkNode<T>> _m_mlinkNodeMap = new ConcurrentHashMap<>();
    private MutexAtom _m_locker = new MutexAtom();

    public TimeKeyLinkList()
    {
        _m_locker.reducePriority(29);
    }

    /*****
     * 根据data的key，快速检索到对应的Node，更新数据并把节点移到List的最尾部
     * @param _data
     */
    public void update(T _data)
    {
        _data.setTimeStamp(CommonFunc.getNowTimeSec());
        _m_locker.lock();
        try
        {
            LinkNode<T> node = _m_mlinkNodeMap.get(_data.getkey());
            if (null == node)
            {
                node = new LinkNode<T>();
                node.data = _data;
                _m_mlinkNodeMap.put(_data.getkey(), node);
            } else
            {
                node.data = _data;
                LinkNode<T> pre = node.preNode;
                LinkNode<T> next = node.nextNode;
                if (pre == null)//头节点
                {
                    _m_firstNode = next;
                } else
                {
                    pre.nextNode = next;
                }

                if (next == null)//尾部节点
                {
                    _m_lastNode = pre;
                } else
                {
                    next.preNode = pre;
                }
            }
            addLast(node);
        } finally
        {
            _m_locker.unlock();
        }
    }

    /*****
     * 加入队列尾部
     * @param _node
     */
    private void addLast(LinkNode<T> _node)
    {

        if (isEmpty())//空表
        {
            _m_firstNode = _node;
            _m_lastNode = _node;
            _node.preNode = _node.nextNode = null;
            return;
        }
        _m_lastNode.nextNode = _node;
        _node.preNode = _m_lastNode;
        _m_lastNode = _node;
        _node.nextNode = null;

    }

    /********
     * 移除指定时间段之前的对象
     * @param _expiredSpanSec
     * @return
     */
    public List<T> popFirstList(int _expiredSpanSec)
    {
        int nowSec = CommonFunc.getNowTimeSec();
        _m_locker.lock();
        try
        {
            List<T> retList = null;
            T data = peekFirst();
            while (data != null)
            {
                if (nowSec - data.getTimeStamp() >= _expiredSpanSec)
                {
                    if (null == retList)
                    {
                        retList = new ArrayList<>();
                    }
                    retList.add(data);
                    popFirst();
                    data = peekFirst();
                } else
                {
                    break;
                }
            }
            return retList;
        } finally
        {
            _m_locker.unlock();
        }

    }

    public boolean isEmpty()
    {
        return _m_firstNode == null;
    }

    /******
     * 移除第一个节点
     * @return
     */
    private T popFirst()
    {

        if (isEmpty()) //空链表
            return null;
        LinkNode<T> retNode = _m_firstNode;
        _m_firstNode = retNode.nextNode;
        if (_m_firstNode == null)
        {
            _m_lastNode = null;//表空了
        } else
        {
            _m_firstNode.preNode = null;
        }
        retNode.preNode = retNode.nextNode = null;//断开链接
        _m_mlinkNodeMap.remove(retNode.data.getkey());
        return retNode.data;

    }

    private T peekFirst()
    {
        return _m_firstNode == null ? null : _m_firstNode.data;
    }

    public int size()
    {
        return _m_mlinkNodeMap.size();
    }

    public T lookupByKey(K key)
    {
        LinkNode<T> node = _m_mlinkNodeMap.get(key);
        return null == node ? null : node.data;
    }

    public T computeIfAbsent(K key, Function<? super K, ? extends T> _createFunc)
    {
        if (key == null || _createFunc == null)
            throw new NullPointerException();
        _m_locker.lock();
        try
        {
            LinkNode<T> node = _m_mlinkNodeMap.get(key);
            if (null == node)
            {
                T data = _createFunc.apply(key);
                return data;
            } else
            {
                return node.data;
            }
        } finally
        {
            _m_locker.unlock();
        }

    }

    /**
     * 通过 Key 值移除节点
     * @param _key K
     */
    public void removeByKey(K _key)
    {
        _m_locker.lock();
        try
        {
            LinkNode<T> loaderNode = _m_mlinkNodeMap.get(_key);
            if (loaderNode == null)
            {
                //不存在这个节点
                return;
            }
            //本节点的上一个节点
            LinkNode<T> preNode = loaderNode.preNode;
            //本节点的下一个节点
            LinkNode<T> nextNode = loaderNode.nextNode;

            //本节点是头结点
            if (preNode == null)
            {
                //将下一个节点设置为头节点
                _m_firstNode = nextNode;
            }
            //本节点是尾结点
            if (nextNode == null)
            {
                //将上一个节点设置为尾结点
                _m_lastNode = preNode;
            }

            if (preNode != null)
            {
                preNode.nextNode = nextNode;
            }
            if (nextNode != null)
            {
                nextNode.preNode = preNode;
            }

            //帮助jvm释放数据
            loaderNode.preNode = null;
            loaderNode.nextNode = null;

            //从map中移除本节点
            _m_mlinkNodeMap.remove(_key);

        } finally
        {
            _m_locker.unlock();
        }
    }

}
