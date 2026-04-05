package NPCommon.Util.Delegate.DelegateStat;

import NPCommon.Util.Delegate.DelegateBase;

import java.lang.ref.WeakReference;

public class DelegateStat
{
    private long _m_id;

    public DelegateStat(DelegateBase _delegate)
    {
        _m_id = _delegate.getId();
        _m_weekRef = new WeakReference<>(_delegate);
    }

    private WeakReference<DelegateBase> _m_weekRef;

    public long getId()
    {
        return _m_id;
    }

    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("[" + getId() + "]\t");
        DelegateBase delegateBase = _m_weekRef.get();
        if (null == delegateBase)
        {
            sb.append("[null]");
        } else
            sb.append(delegateBase.toString());
        return sb.toString();
    }


    public boolean isEmpty()
    {
        return _m_weekRef.get() == null;
    }

    public int getHandlerCount()
    {
        DelegateBase delegateBase = _m_weekRef.get();
        if (null == delegateBase)
        {
            return 0;
        }
        return delegateBase.getHandlerCount();
    }

    public DelegateBase getDelegate()
    {
        return _m_weekRef.get();
    }
}
