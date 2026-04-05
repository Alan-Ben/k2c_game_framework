package NPCommon.CommonObj.ShowItemCollector;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Property._TBaseEnumObj;

import java.util.*;

/**
 * 展示道具的收集器（该物品需要有对应的枚举）
 * @param <T> 展示类型枚举
 * @param <P> 展示结构体
 */
public abstract class _AShowItemCollector<T extends Enum<T>, P extends _IALProtocolStructure>
{
    //是否需要合并数值
    protected boolean _m_needMerge;
    //实例(可以是大臣id、妃子id、宠物id)和物品数量数组的map
    protected Map<Long, List<ItemValueCounter<T>>> _m_map;
    //枚举包装对象
    protected _TBaseEnumObj<T> _m_eoEnumObj;

    public _AShowItemCollector(Class<T> _enumClass, boolean _needMerge)
    {
        _m_needMerge = _needMerge;
        _m_map = new HashMap<>();
        _m_eoEnumObj = _TBaseEnumObj.getBaseEnum(_enumClass);
    }

    /**
     * 返回实例对应的数组对象
     * @param _instanceId 实例id
     * @return 数组
     */
    public List<ItemValueCounter<T>> ensure(long _instanceId)
    {
        return _m_map.computeIfAbsent(_instanceId, k -> new ArrayList<>());
    }

    /**
     * 记录获得
     * @param _instanceId 实例id
     * @param _type       展示枚举
     * @param _num        数量
     */
    public void record(long _instanceId, T _type, int _num)
    {
        List<ItemValueCounter<T>> list = ensure(_instanceId);

        ensureValueCollector(list, _type).add(_num);
    }

    /**
     * 确保对应的收集器
     * @param _list 收集器列表
     * @param _type 展示枚举
     * @return 收集器
     */
    public ItemValueCounter<T> ensureValueCollector(List<ItemValueCounter<T>> _list, T _type)
    {
        ItemValueCounter<T> collector = null;
        for (ItemValueCounter<T> tmp : _list)
        {
            if (tmp.getType() == _type)
            {
                collector = tmp;
                break;
            }
        }

        if (collector == null)
        {
            collector = new ItemValueCounter<>(_type);
            _list.add(collector);
        }

        return collector;
    }

    /**
     * 构造展示结构体
     * @param _instanceId 实例id
     * @param _type      展示枚举
     * @param _num        数量
     * @return
     */
    public abstract P makeSub(long _instanceId, T _type, int _num);

    /**
     * 返回对应的展示结构体列表
     * @return 展示结构体列表
     */
    public List<P> makeProto()
    {
        List<P> list = new ArrayList<>();
        if (_m_map.isEmpty())
            return list;

        //遍历map构造展示列表
        Set<Map.Entry<Long, List<ItemValueCounter<T>>>> entries = _m_map.entrySet();
        for (Map.Entry<Long, List<ItemValueCounter<T>>> entry : entries)
        {
            //对应物品数量的数组
            List<ItemValueCounter<T>> collectorList = entry.getValue();
            for (ItemValueCounter<T> collector : collectorList)
            {
                if (_m_needMerge)
                {
                    list.add(makeSub(entry.getKey(), collector.getType(), collector.getTotalValue()));
                } else
                {
                    List<Integer> valueList = collector.getList();
                    if (valueList.isEmpty())
                        continue;

                    //遍历数量列表构造展示结构体
                    for (Integer value : valueList)
                    {
                        list.add(makeSub(entry.getKey(), collector.getType(), value));
                    }
                }
            }
        }
        return list;
    }
}
