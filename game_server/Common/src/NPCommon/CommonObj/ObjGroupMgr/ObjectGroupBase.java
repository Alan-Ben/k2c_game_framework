package NPCommon.CommonObj.ObjGroupMgr;

import java.util.*;

public abstract class ObjectGroupBase<K, V>
{
    private Map<K, V> objectMap;
    private ArrayList<V> objectList;

    public ObjectGroupBase()
    {
        objectMap = new HashMap<>();
        objectList = new ArrayList<>();
    }

    public V get(K key)
    {
        return objectMap.get(key);
    }

    /**
     * 获取第一个
     * @return
     */
    public V getFirst()
    {
        if (objectList.isEmpty())
        {
            return null;
        }
        return objectList.get(0);
    }

    public int size()
    {
        return objectMap.size();
    }

    /**
     * 获取下一个
     * @param val
     * @return
     */
    public V getNext(V val)
    {
        int index = objectList.indexOf(val);

        if (index != -1 && index < objectList.size() - 1)
        {
            // 如果索引有效，获取下一个元素
            return objectList.get(index + 1);
        } else
        {
            return null;
        }
    }

    /**
     * 获取最后一个
     * @return
     */
    public V getLast()
    {
        int lastIndex = objectList.size() - 1;
        if (lastIndex >= 0)
        {
            return objectList.get(lastIndex);
        } else
        {
            return null;
        }
    }

    /**
     * 是否是最后一个
     * @param val
     * @return
     */
    public boolean isLast(V val)
    {
        int lastIndex = objectList.size() - 1;
        return objectList.get(lastIndex).equals(val);
    }

    public void add(K key, V value)
    {
        objectMap.put(key, value);
        objectList.add(value);
        objectList.sort(getCompareFunc());
    }

    public Set<K> keySet()
    {
        return objectMap.keySet();
    }

    public ArrayList<V> getAll()
    {
        return objectList;
    }

    protected abstract Comparator<? super V> getCompareFunc();
}