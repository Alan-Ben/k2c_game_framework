package NPCommon.Util.NpServerObj;

import java.util.HashMap;
import java.util.Set;

/**
 * @description: 为数据结构是 二维数组的内容提供 map 一样的操作函数
 * 目的是从 bytebuffer 读取出的数据结构能像 map 一样简单操作
 * E entry 键值对
 * K 键值
 * V 值
 * @author: ricci
 * @date: 2022-05-12 11:12:17
 */
public abstract class _ACommArrayToMap<E, K, V>
{

    public _ACommArrayToMap()
    {

    }

    /**
     * 返回一个等效 HashMap
     * @return HashMap<K, V>
     */
    public HashMap<K, V> copyToMap()
    {
        HashMap<K, V> copyMap = new HashMap<>();
        for (E entry : getEntrySet())
        {
            if (entry == null)
            {
                continue;
            }
            copyMap.put(getKey(entry), getValue(entry));
        }
        return copyMap;
    }


    /**
     * 根据键获取值
     * @param _key 指定键
     * @return 值
     */
    public V get(Long _key)
    {
        for (E entry : getEntrySet())
        {
            if (entry == null)
            {
                continue;
            }
            if (getKey(entry) == _key)
            {
                return getValue(entry);
            }
        }
        return null;
    }

    /**
     * 根据键获得键值对
     * @param _key 键
     * @return E
     */
    public E getEntry(K _key)
    {
        for (E entry : getEntrySet())
        {
            if (entry == null)
            {
                continue;
            }
            if (getKey(entry) == _key)
            {
                return entry;
            }
        }
        return null;
    }

    /**
     * 增加或修改map中的值
     * @param _key   键
     * @param _value 值
     */
    public void put(K _key, V _value)
    {
        E entry = getEntry(_key);
        if (entry == null)
        {
            entry = createNewEntry(_key, _value);
            putInMap(entry);
        } else
        {
            setValue(entry, _value);
        }
    }

    /**
     * 获取entry集合
     * @return Set<E>
     */
    public abstract Set<E> getEntrySet();

    /**
     * 从entry中获取键
     * @param _entry 键值对
     * @return K
     */
    protected abstract K getKey(E _entry);

    /**
     * 从entry中获取值
     * @param _entry 键值对
     * @return V
     */
    protected abstract V getValue(E _entry);

    /**
     * 为entry设置值
     * @param _entry 键值对
     * @param _value 设置值
     */
    protected abstract void setValue(E _entry, V _value);


    /**
     * 创建
     * @param _key   键
     * @param _value 值
     */
    protected abstract E createNewEntry(K _key, V _value);

    /**
     * 放入实际的Map中
     * @param _entry E
     */
    protected abstract void putInMap(E _entry);

    /**
     * 保存所有变更数据
     */
    public abstract void saveAllMark();

}
