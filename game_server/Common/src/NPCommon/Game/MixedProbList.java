package NPCommon.Game;

import NPCommon.Util.CommonFunc;

/**
 * 混合概率随机列表
 * 支持绝对概率（rand_pro，万分比）和权重（rand_wei）两种方式共存
 * 总概率关系：10000 = Σ(rand_pro) + totalWei × perWeightPro
 *
 * 随机逻辑：
 * 1. r ∈ [0, 9999]
 * 2. r < totalPro：命中绝对概率区间，从绝对概率列表中按 rand_pro 比例随机
 * 3. r >= totalPro：命中权重区间，从权重列表中按 rand_wei 比例随机
 */
public class MixedProbList<T>
{
    // 绝对概率列表，以 rand_pro 作为权重
    private WeightValueList<T> _m_absList    = new WeightValueList<>();
    // 权重列表，以 rand_wei 作为权重
    private WeightValueList<T> _m_weightList = new WeightValueList<>();
    // 所有 rand_pro 之和（万分比）
    private int _m_totalPro = 0;

    /**
     * 添加一个元素
     * @param _value   元素对象
     * @param _randPro 绝对概率（万分比），0表示不参与绝对概率
     * @param _randWei 随机权重，0表示不参与权重随机
     */
    public void add(T _value, int _randPro, int _randWei)
    {
        if (_randPro > 0)
        {
            _m_absList.add(_value, _randPro);
            _m_totalPro += _randPro;
        }
        if (_randWei > 0)
            _m_weightList.add(_value, _randWei);
    }

    /**
     * 随机返回一个元素
     */
    public T random()
    {
        int r = CommonFunc.randomInt(9999);
        if (r < _m_totalPro)
            return _m_absList.random();
        return _m_weightList.random();
    }

    public boolean isEmpty()
    {
        return _m_absList.isEmpty() && _m_weightList.isEmpty();
    }
}
