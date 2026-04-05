package NPCommon.Util;

import java.util.Collection;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

/**
 * @description:
 * @author: ricci
 * @date: 2023-02-04 15:48:36
 */
public class CardinalityList
{
    private final Collection<?> _m_collection;

    public CardinalityList(Collection<?> _collection)
    {
        _m_collection = _collection;
    }

    public int size()
    {
        return _m_collection.size();
    }

    public Set<Object> keySet()
    {
        return new HashSet<>(_m_collection);
    }

    public Iterator<?> iterator()
    {
        return _m_collection.iterator();
    }
}
