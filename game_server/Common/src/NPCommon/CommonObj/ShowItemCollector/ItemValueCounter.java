package NPCommon.CommonObj.ShowItemCollector;

import java.util.ArrayList;
import java.util.List;

public class ItemValueCounter<T extends Enum<T>>
{
    //类型
    private T _m_type;
    //总值
    private int _m_totalValue;
    //值列表
    private List<Integer> _m_list;

    public ItemValueCounter(T _type)
    {
        _m_type = _type;
        _m_totalValue = 0;
        _m_list = new ArrayList<>();
    }

    /**
     * 获取类型
     * @return
     */
    public T getType()
    {
        return _m_type;
    }

    /**
     * 获取总值
     * @return
     */
    public int getTotalValue()
    {
        return _m_totalValue;
    }

    /**
     * 获取列表
     * @return
     */
    public List<Integer> getList()
    {
        return _m_list;
    }

    public void add(int _num)
    {
        _m_totalValue += _num;
        _m_list.add(_num);
    }
}
