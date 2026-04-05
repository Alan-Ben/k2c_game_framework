package NPCommon.Util.Delegate;

/***
 * 监听入口
 * @param <T>
 */
public class HandlerEntry<T extends HandlerBase> extends HandlerEntryBase
{
    private T _m_handler;

    public HandlerEntry(T _handler)
    {
        _m_handler = _handler;
    }

    @Override
    public T getHandler()
    {
        return _m_handler;
    }

}
