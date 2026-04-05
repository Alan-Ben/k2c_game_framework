package ActivitiesV01.Activities.TileMatchActivity.Game;

import java.util.Collection;


/**
 * 数据结构是 一维数组的内容提供 ArrayList 一样的操作函数
 * 目的是从 bytebuffer 读取出的数据结构能像 ArrayList 一样简单操作
 * K 键值
 * V 值
 */
public abstract class _ATileMatchArrayToList<K, V>
{

    public V get(K _key)
    {
        for (V v : getList())
        {
            if (getIndex(v).equals(_key))
            {
                return v;
            }
        }
        return null;
    }

    /**
     * 向列表中新增数据
     * @param _v
     */
    abstract void add(V _v);

    /**
     * 从数据中获得键值
     * @param _v 数据
     * @return
     */
    abstract K getIndex(V _v);

    /**
     * 获取列表中所有数据
     * @return
     */
    abstract Collection<V> getList();

    /**
     * 设置值
     * @param _v
     */
    abstract void set(K _key, V _v);

    /**
     * 保存所有变更数据
     */
    abstract void saveAllMark();

}
