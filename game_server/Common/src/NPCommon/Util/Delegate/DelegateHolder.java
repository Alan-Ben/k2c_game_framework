package NPCommon.Util.Delegate;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/*******
 * 监听多个Delegate，统一进行Clear，特别用于活动结束统一清除对全局Delegate的监听，避免遗漏
 */
public class DelegateHolder implements _IHandlerHolder
{
    private Map<DelegateBase, Integer> _m_delMap = new ConcurrentHashMap<>();//监听列表，用ConcurrentHashMap保证唯一和线程安全

    public void connect(ADelegateNone _del, HandlerNone _handler)
    {
        _del.addHandler(this, _handler);
        _m_delMap.put(_del, 0);
    }

    public <T> void connect(ADelegateOne<T> _del, HandlerOne<T> _handler)
    {
        _del.addHandler(this, _handler);
        _m_delMap.put(_del, 0);
    }

    public <T1, T2> void connect(ADelegateTwo<T1, T2> _del, HandlerTwo<T1, T2> _handler)
    {
        _del.addHandler(this, _handler);
        _m_delMap.put(_del, 0);
    }

    public <T1, T2, T3> void connect(ADelegateThree<T1, T2, T3> _del, HandlerThree<T1, T2, T3> _handler)
    {
        _del.addHandler(this, _handler);
        _m_delMap.put(_del, 0);
    }

    public <T1, T2, T3, T4> void connect(ADelegateFour<T1, T2, T3, T4> _del, HandlerFour<T1, T2, T3, T4> _handler)
    {
        _del.addHandler(this, _handler);
        _m_delMap.put(_del, 0);
    }

    /****
     * 销毁所有的监听
     */
    public void clear()
    {
        for (DelegateBase del : _m_delMap.keySet())
        {
            del.clear(this);
        }
    }
}
