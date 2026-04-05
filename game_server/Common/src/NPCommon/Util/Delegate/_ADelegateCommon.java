package NPCommon.Util.Delegate;


import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

/*****
 * 抽象类，提供Delegate的通用方法
 * @param <T>
 */
public abstract class _ADelegateCommon<T extends HandlerBase> extends DelegateBase
{
    protected final List<HandlerEntry<T>> _m_entryList = new ArrayList<>(); //调用节点列表

    public _ADelegateCommon(Object _parent)
    {
        super(_parent);
    }

    @Override
    public int getHandlerCount()
    {
        synchronized (_m_entryList)
        {
            return _m_entryList.size();
        }
    }

    @Override
    public List<HandlerEntryBase> getEntryList()
    {
        synchronized (_m_entryList)
        {
            return new ArrayList<>(_m_entryList);
        }
    }

    public HandlerEntryBase addHandler(_IHandlerHolder _holder, T handler)
    {
        if (null == handler)
            return null;
        HandlerEntry<T> entry = new HandlerEntry<>(handler);
        entry.setAddTime(CommonFunc.getNowTimeSec());
        entry.setHolder(_holder);
        synchronized (_m_entryList)
        {
            _m_entryList.add(entry);
        }

        if (isGlobal() && null == _holder)
        {
            CommLog.error("========handler of null holder add to Global Delegate!!!!!!", new Exception());
        }
        return entry;
    }

    protected List<T> getHandlerList()
    {
        List<T> handlers;
        synchronized (_m_entryList)
        {
            if (_m_entryList.isEmpty())
                return null;

            handlers = new ArrayList<>();
            for (HandlerEntry<T> entry : _m_entryList)
            {
                handlers.add(entry.getHandler());
            }
        }
        return handlers;
    }

    @Override
    public int clear()
    {
        synchronized (_m_entryList)
        {
            int count = _m_entryList.size();
            _m_entryList.clear();
            return count;
        }

    }

    public void clear(_IHandlerHolder _holder)
    {
        synchronized (_m_entryList)
        {
            for (int i = _m_entryList.size() - 1; i >= 0; i--)
            {
                if (_m_entryList.get(i).getHolder() == _holder)
                {
                    _m_entryList.remove(i);
                }
            }
        }

    }

    @Override
    public boolean removeEntry(HandlerEntryBase _entry)
    {
        synchronized (_m_entryList)
        {
            return _m_entryList.remove(_entry);
        }
    }
}
