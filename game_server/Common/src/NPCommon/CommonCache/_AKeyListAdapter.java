package NPCommon.CommonCache;

import java.util.List;

public abstract class _AKeyListAdapter<T> implements _IListAdapter<Long>
{
    List<T> _m_itemList;

    public _AKeyListAdapter(List<T> _itemList)
    {
        _m_itemList = _itemList;
    }

    @Override
    public int size()
    {
        return _m_itemList.size();
    }

    @Override
    public Long get(int _i)
    {
        T item = _m_itemList.get(_i);
        return getKey(item);
    }

    public abstract long getKey(T _item);
}