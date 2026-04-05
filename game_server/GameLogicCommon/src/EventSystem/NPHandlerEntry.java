package EventSystem;

import NPCommon.Util.Delegate.HandlerEntryBase;

import java.lang.ref.WeakReference;

/**
 * @description: 单个 handler 的在 NPPlayerEventHandlerMgr 的标识
 * @author: ricci
 * @date: 2022-07-27 13:50:19
 */
public class NPHandlerEntry<T>
{
    /**
     * 事件id
     */
    private int _m_eventId;
    /**
     * Delegate 所在对象
     */
    private NPEventHandlerInfo<T> _m_eventHandlerInfo;

    /**
     * 在 Delegate 中的 handler 对象
     */
    private HandlerEntryBase _m_handlerEntry;
    /**
     * 所在holderMap
     */
    private WeakReference<NPHolderHandlerMap<T>> _m_handlerHolderMap;

    protected NPHandlerEntry(int _eventId, NPEventHandlerInfo<T> _info,
                             HandlerEntryBase _handlerEntry, NPHolderHandlerMap<T> _handlerHolderMap)
    {
        this._m_eventId = _eventId;
        this._m_eventHandlerInfo = _info;
        this._m_handlerEntry = _handlerEntry;
        this._m_handlerHolderMap = new WeakReference<>(_handlerHolderMap);
    }

    protected int getEventId()
    {
        return _m_eventId;
    }

    protected NPEventHandlerInfo<T> getEventHandlerInfo()
    {
        return _m_eventHandlerInfo;
    }

    protected HandlerEntryBase getHandlerEntry()
    {
        return _m_handlerEntry;
    }

    protected NPHolderHandlerMap<T> getHandlerHolderMap()
    {
        return _m_handlerHolderMap.get();
    }

    /**
     * 从注册的 delegate 中释放本对象指定的 handler
     */
    protected boolean disposeHandler()
    {
        return _m_eventHandlerInfo.unregHandler(getHandlerEntry());
    }

    /**
     * 销毁
     * @return boolean
     */
    public boolean dispose()
    {
        if (getHandlerHolderMap() != null)
        {
            //移除holder对本对象的持有
            getHandlerHolderMap().removeEntry(this);
        }

        //销毁 delegate 中的 handler
        return disposeHandler();
    }
}
