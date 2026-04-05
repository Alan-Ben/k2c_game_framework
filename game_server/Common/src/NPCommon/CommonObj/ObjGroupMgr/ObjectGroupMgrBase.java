package NPCommon.CommonObj.ObjGroupMgr;

import java.util.*;

public abstract class ObjectGroupMgrBase<GROUP_KEY, K, V>
{
    protected LinkedHashMap<GROUP_KEY, ObjectGroupBase<K, V>> map;

    public ObjectGroupMgrBase()
    {
        map = new LinkedHashMap<>();
    }

    public V get(GROUP_KEY groupKey, K key)
    {
        ObjectGroupBase<K, V> kvObjectGroupBase = map.get(groupKey);
        if (kvObjectGroupBase == null)
        {
            return null;
        }
        return kvObjectGroupBase.get(key);
    }

    public ObjectGroupBase<K, V> getGroup(GROUP_KEY groupKey)
    {
        return map.get(groupKey);
    }

    public Set<GROUP_KEY> keySet()
    {
        return map.keySet();
    }

    public Collection<ObjectGroupBase<K, V>> values()
    {
        return map.values();
    }

    public void ensure(GROUP_KEY groupKey, K key, V value)
    {
        ObjectGroupBase<K, V> kvObjectGroupBase = map.get(groupKey);
        if (kvObjectGroupBase == null)
        {
            kvObjectGroupBase = createGroup(groupKey);
            map.put(groupKey, kvObjectGroupBase);
        }
        kvObjectGroupBase.add(key, value);
    }

    public Map.Entry<GROUP_KEY, ObjectGroupBase<K, V>> getFirstEntry()
    {
        return map.entrySet().stream().findFirst().orElse(null);
    }

    public Map.Entry<GROUP_KEY, ObjectGroupBase<K, V>> getLastEntry()
    {
        if (map.isEmpty())
        {
            return null;
        }
        return map.entrySet().stream().reduce((o1, o2) -> o2).orElse(null);
    }

    /**
     * 获取下一个组
     * @param groupKey
     * @return
     */
    public Map.Entry<GROUP_KEY, ObjectGroupBase<K, V>> getNext(GROUP_KEY groupKey)
    {
        if (map.isEmpty())
        {
            return null;
        }
        Iterator<Map.Entry<GROUP_KEY, ObjectGroupBase<K, V>>> iterator = map.entrySet().iterator();

        // 这是当前元素
        Map.Entry<GROUP_KEY, ObjectGroupBase<K, V>> currentEntry = null;

        while (iterator.hasNext())
        {
            // 获取当前元素
            currentEntry = iterator.next();
            if (currentEntry.getKey().equals(groupKey))
            {

                // 如果还有下一个元素
                if (iterator.hasNext())
                {
                    // 获取下一个元素
                    return iterator.next();
                }
            }
        }
        return null;
    }

    public int getSize()
    {
        return map.size();
    }

    public int getGroupSize(GROUP_KEY groupKey)
    {
        return getGroup(groupKey).size();
    }

    public void autoLoad(Collection<V> collection)
    {
        clear();
        collection.forEach(e -> ensure(getGroupKey(e), getKey(e), e));
    }

    public void clear()
    {
        map.clear();
    }

    protected abstract ObjectGroupBase<K, V> createGroup(GROUP_KEY groupKey);

    protected abstract GROUP_KEY getGroupKey(V val);

    protected abstract K getKey(V val);

}

