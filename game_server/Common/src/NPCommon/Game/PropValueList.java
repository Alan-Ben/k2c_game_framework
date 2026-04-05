package NPCommon.Game;

import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;

import java.util.ArrayList;
import java.util.List;

public class PropValueList<T>
{
    public static class ProbabilityValue<T>
    {
        //item
        public T value;
        //概率值
        public long prop;

        @Override
        public String toString()
        {
            if (value != null)
            {
                return String.format("%s:%d", value.toString(), prop);
            } else
            {
                return String.format("null:%d", prop);
            }
        }

        public ProbabilityValue(T value, long prop)
        {
            this.value = value;
            this.prop = prop;
        }

        public ProbabilityValue<T> duplicate()
        {
            return new ProbabilityValue<>(value, prop);
        }
    }

    private List<ProbabilityValue<T>> _m_list = new ArrayList<>();

    /******
     * 随机并移除
     * @return
     */
    public T randomAndRemove()
    {
        if (isEmpty())
            return null;

        //随机概率用于掉落计算
        long rand = CommonFunc.randomInt(10000);

        for (int i = 0; i < _m_list.size(); i++)
        {
            long pro = _m_list.get(i).prop;
            if (pro < rand)
            {
                rand -= pro;
            } else//随机数落在i,第个物品掉落成功了
            {
                return _m_list.remove(i).value;
            }
        }

        return null;
    }

    public boolean isEmpty()
    {
        return _m_list.isEmpty();
    }

    public void add(T value, long weight)
    {
        _m_list.add(new ProbabilityValue<>(value, weight));
    }

    /*****
     * 复制一份新的随机列表
     * @return
     */
    public PropValueList<T> duplicate()
    {
        PropValueList<T> ret = new PropValueList<T>();
        for (ProbabilityValue<T> tWeightValue : _m_list)
        {
            ret._m_list.add(tWeightValue.duplicate());
        }
        return ret;
    }

    /****
     * 返回随机列表大小
     * @return
     */
    public int size()
    {
        return _m_list.size();
    }

    /******
     * 清空随机列表
     */
    public void clear()
    {
        _m_list.clear();
    }

    @Override
    public String toString()
    {
        return StringFunc.list2String(_m_list);
    }
}
