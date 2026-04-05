package EventSystem;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.Delegate.HandlerEntryBase;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.LogicEvent._ALogicEventBase;

import java.util.HashMap;
import java.util.WeakHashMap;

/**
 * 事件响应管理器的抽象基类
 * 该管理类通过维护两个容器实现事件的监听注册和销毁，和事件触发的通知功能
 * 1._m_hmEventHandlerMap维护了事件id和事件管理对象的映射关系，事件管理对象存储了对指定事件id进行监听的监听信息列表，存储了发起监听对象和对应的回调函数
 * 2._m_hmHolderHandlerMap是一个弱引用map，主要功能是防止由于忘记反注册监听导致的内存泄漏。通过存储发起监听对象和注册监听信息NPHandlerEntry（存储了事件管理对象）列表，来保证忘记反注册后的对象正常卸载。
 * 在发生垃圾回收时，弱引用map如果有key是要被回收，那么对应键值的对象也会被回收，通过对应值的析构函数，对该发起监听对象注册过的监听进行反注册。
 * @param <T> 触发事件的主体对象
 */
public abstract class _ANPEventHandlerMgr<T>
{
    //监听的事件管理 <事件id,事件管理对象>
    private HashMap<Integer, NPEventHandlerInfo<T>> _m_hmEventHandlerMap;
    //监听事件与发起监听的对象关系表,弱引用map，键值被释放后，map中的value也被释放
    private WeakHashMap<_IHandlerHolder, NPHolderHandlerMap<T>> _m_hmHolderHandlerMap;
    //锁对象
    private MutexAtom _m_mutex;

    public _ANPEventHandlerMgr()
    {
        _m_hmEventHandlerMap = new HashMap<>();
        _m_hmHolderHandlerMap = new WeakHashMap<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock() { _m_mutex.lock(); }
    private void _unlock() { _m_mutex.unlock(); }

    /**
     * 处理监听响应事件
     * @param _evt
     * @param _t1
     */
    public void handle(_ALogicEventBase _evt, T _t1)
    {
        //取出对应的执行事件对象
        NPEventHandlerInfo<T> handlerInfo = null;

        _lock();

        try
        {
            handlerInfo = _m_hmEventHandlerMap.get(_evt.getEventId());
        } finally
        {
            _unlock();
        }

        //如果存在对应的执行事件，则响应handler
        if (null != handlerInfo)
        {
            handlerInfo._handle(_evt, _t1);
        }
    }

    /**
     * 注册监听事件管理对象
     * @param _evtId 事件id
     * @param _holder 发起监听的对象
     * @param _handler 回调对象
     * @return 监听注册信息
     */
    public NPHandlerEntry<T> regHandler(int _evtId, _IHandlerHolder _holder, HandlerTwo<_ALogicEventBase, T> _handler)
    {
        _lock();

        try
        {
            //注册需要监听的事件
            NPEventHandlerInfo<T> info = _m_hmEventHandlerMap.get(_evtId);
            if (null == info)
            {
                info = new NPEventHandlerInfo<T>();
                _m_hmEventHandlerMap.put(_evtId, info);
            }

            //注册回调
            HandlerEntryBase handlerEntryBase = info._regDelegate(_holder, _handler);

            //记录holder对应的事件列表，用于移除监听
            NPHolderHandlerMap<T> holderHandlerMap = _ensureHolder(_holder);
            //创建 handler 的注册信息
            NPHandlerEntry<T> handlerEntry = new NPHandlerEntry<T>(_evtId, info, handlerEntryBase, holderHandlerMap);

            holderHandlerMap._addHandleInfo(handlerEntry);
            return handlerEntry;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册事件与对应对象的映射关系
     * @param _holder 发起监听的对象
     * @return 该对象发起监听的事件列表对象
     */
    private NPHolderHandlerMap<T> _ensureHolder(_IHandlerHolder _holder)
    {
        _lock();

        try
        {
            NPHolderHandlerMap<T> map = _m_hmHolderHandlerMap.get(_holder);
            if (null == map)
            {
                map = new NPHolderHandlerMap<T>(_holder);
                _m_hmHolderHandlerMap.put(_holder, map);
            }

            return map;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销监听事件与对象的映射关系
     * @param _holder 发起监听的对象
     */
    public void unregHandler(_IHandlerHolder _holder)
    {
        _lock();

        try
        {
            //查找该对象之前注册监听的信息
            NPHolderHandlerMap<T> map = _m_hmHolderHandlerMap.remove(_holder);
            if (null == map)
                return;

            //清除该对象下注册的所有监听
            map._clearHandler();
            //标记为手动清除，避免垃圾回收时误报
            map.setOptUnReg(true);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销监听事件
     * @param _handlerEntry 监听注册信息
     * @return 监听事件卸载是否成功
     */
    public boolean unregHandler(NPHandlerEntry<T> _handlerEntry)
    {
        _lock();

        try
        {
            if (_handlerEntry == null)
            {
                return false;
            }
            //查找对应的事件管理对象
            NPEventHandlerInfo<T> info = _m_hmEventHandlerMap.get(_handlerEntry.getEventId());
            if (null == info)
            {
                return true;
            }
            info.unregHandler(_handlerEntry.getHandlerEntry());
            //移除 holderMap里存放的 entry
            NPHolderHandlerMap<T> map = _handlerEntry.getHandlerHolderMap();
            if (map == null)
            {
                return true;
            }
            map.removeEntry(_handlerEntry);

            //所有监听已经全部移除时，反注册holder
            if (map.isEmpty())
            {
                unregHandler(map.getHolder());
            }

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁监听数据
     */
    public void dispose()
    {
        _lock();

        try
        {
            _m_hmEventHandlerMap.clear();
            for (NPHolderHandlerMap<T> value : _m_hmHolderHandlerMap.values())
            {
                if (value == null)
                    continue;

                value._clearHandler();
            }
            _m_hmHolderHandlerMap.clear();
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        return "NPPlayerEventHandlerMgr{" +
                "_m_hmHolderHandlerMap=" +
                _m_hmHolderHandlerMap.size() +
                '}';
    }
}
