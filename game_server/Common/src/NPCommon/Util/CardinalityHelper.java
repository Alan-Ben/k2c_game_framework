package NPCommon.Util;

import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

/**
 * @description: 比较两个集合元素组成是否一致帮助类
 * @author: ricci
 * @date: 2023-02-04 15:47:14
 */
public class CardinalityHelper
{
    public CardinalityList _m_cardinalityA;
    public CardinalityList _m_cardinalityB;

    public CardinalityHelper(Collection<?> _a, Collection<?> _b)
    {
        _m_cardinalityA = new CardinalityList(_a);
        _m_cardinalityB = new CardinalityList(_b);
    }

    // 统计元素的个数
    public static Map<Object, Integer> getCardinalityMap(Iterator<?> coll)
    {
        final Map<Object, Integer> count = new HashMap<>();
        while (coll.hasNext())
        {
            Object obj = coll.next();
            count.merge(obj, 1, Integer::sum);
        }
        return count;
    }

    // 获得每个元素出现的频次
    private int getFreq(final Object obj, final Map<Object, Integer> freqMap)
    {
        final Integer count = freqMap.get(obj);
        if (count != null)
        {
            return count;
        }
        return 0;
    }

    /**
     * 获得A集合的某元素出现频次
     * @param _obj 指定元素
     * @return int
     */
    public int freqA(Object _obj)
    {
        Iterator<?> iterator = _m_cardinalityA.iterator();
        //统计元素个数的map
        Map<Object, Integer> cardinalityMap = getCardinalityMap(iterator);
        return getFreq(_obj, cardinalityMap);
    }

    /**
     * 获得B集合的某元素出现频次
     * @param _obj 指定元素
     * @return int
     */
    public int freqB(Object _obj)
    {
        Iterator<?> iterator = _m_cardinalityB.iterator();
        //统计元素个数的map
        Map<Object, Integer> cardinalityMap = getCardinalityMap(iterator);
        return getFreq(_obj, cardinalityMap);
    }
}
