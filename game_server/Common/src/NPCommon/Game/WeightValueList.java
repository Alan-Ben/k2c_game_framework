package NPCommon.Game;

import NPCommon.Log.CommLog;
import NPCommon.RefData.AbstractRefDataMgr;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.RefWrap;
import NPCommon.Util.StringFunc;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class WeightValueList<T>
{
    public static class WeightValue<T>
    {

        public T value;

        public long weight;

        @Override
        public String toString()
        {
            if (value != null)
            {
                return String.format("%s:%d", value.toString(), weight);
            } else
                return String.format("null:%d", weight);
        }

        public WeightValue()
        {
        }

        public WeightValue(T value, long weight)
        {
            this.value = value;
            this.weight = weight;
        }

        public WeightValue<T> duplicate()
        {
            return new WeightValue<>(value, weight);
        }

    }
    private List<WeightValue<T>> _m_list = new ArrayList<>();

    private long _m_lTotalWeight = 0;
    public long getTotalWeight()
    {
        return _m_lTotalWeight;
    }

    public List<WeightValue<T>> getList()
    {
        return _m_list;
    }

    @SuppressWarnings({"rawtypes", "unchecked"})
    public boolean fromString(Class clazz, String sValue)
    {
        String[] steps = CommonFunc.charSplit(sValue, ';');
        for (String str : steps)
        {
            String[] infos = CommonFunc.charSplit(str, ':');
            if (infos.length == 2)
            {
                WeightValue<T> item = new WeightValue<T>();
                try
                {
                    Field valueFiled = item.getClass().getField("value");
                    item.value = (T) AbstractRefDataMgr.parseCreateObj(clazz, infos[0].trim(), "", valueFiled.getName());
                } catch (Exception e)
                {
                    CommLog.error("can not parse value for str:{} of type:{}", infos[0], new Exception());
                    continue;
                }

                item.weight = Long.parseLong(infos[1].trim());
                _m_list.add(item);
                _m_lTotalWeight += item.weight;
            } else
            {
                CommLog.error("WeightValueList 参数数量不为2 str:{} ", sValue);
                return false;
            }
        }
        return true;
    }

    /**
     * 随机返回一个对象
     * @return
     */
    public T random()
    {
        if (_m_lTotalWeight <= 0) return null;
        if (isEmpty()) return null;
        long randValue = CommonFunc.randomLong(_m_lTotalWeight);
        for (int i = 0; i < _m_list.size(); i++)
        {
            long weight = _m_list.get(i).weight;
            if (weight <= 0)
            {
                //不能选取权重小于等于0的对象
                continue;
            }
            randValue -= weight;
            if (randValue <= 0)
            {
                return _m_list.get(i).value;
            }
        }
        return null;
    }

    /**
     * 随机返回一个对象 (增加额外权重)
     * @return
     */
    public T randomWithExtraWeight(T _value, long _extraWeight)
    {
        long newTotalWeight = _m_lTotalWeight + _extraWeight;

        if (newTotalWeight <= 0)
            return null;

        if (isEmpty())
            return null;

        long randValue = CommonFunc.randomLong(newTotalWeight);

        for (int i = 0; i < _m_list.size(); i++)
        {
            WeightValue<T> weightValue = _m_list.get(i);

            long weight = weightValue.weight;
            //不能选取权重小于等于0的对象
            if (weight <= 0)
                continue;

            randValue -= weight;
            if (weightValue.value == _value)
                randValue -= _extraWeight;

            if (randValue <= 0)
                return weightValue.value;
        }
        return null;
    }

    /**
     * 随机返回一个对象(使用自定义随机数生成器)
     * @param _random 随机数生成器
     * @return
     */
    public T random(Random _random, RefWrap<Long> _randomResult)
    {
        if (_m_lTotalWeight <= 0)
            return null;

        if (isEmpty())
            return null;

        long randValue = CommonFunc.randomLong(_m_lTotalWeight, _random);

        if (_randomResult != null)
            _randomResult.set(randValue);

        for (int i = 0; i < _m_list.size(); i++)
        {
            long weight = _m_list.get(i).weight;
            if (weight <= 0)
            {
                //不能选取权重小于等于0的对象
                continue;
            }
            randValue -= weight;
            if (randValue <= 0)
            {
                return _m_list.get(i).value;
            }
        }
        return null;
    }

    /**
     * 随机后返回在列表中的位置
     * @return int
     */
    public int randomReturnIndex()
    {
        if (_m_lTotalWeight <= 0) return -1;
        if (isEmpty()) return -1;
        long randValue = CommonFunc.randomLong(_m_lTotalWeight);
        for (int i = 0; i < _m_list.size(); i++)
        {
            long weight = _m_list.get(i).weight;
            if (weight <= 0)
            {
                //不能选取权重小于等于0的对象
                continue;
            }
            randValue -= weight;
            if (randValue <= 0)
            {
                return i;
            }
        }
        return -1;
    }

    /******
     * 随机并移除
     * @return
     */
    public T randomAndRemove()
    {
        if (_m_lTotalWeight <= 0) return null;
        if (isEmpty()) return null;
        long randValue = CommonFunc.randomLong(_m_lTotalWeight);
        for (int i = 0; i < _m_list.size(); i++)
        {
            WeightValue<T> wei = _m_list.get(i);
            if (wei.weight <= 0)
            {
                //无法选取权重小于等于0的对象
                continue;
            }
            randValue -= wei.weight;
            if (randValue <= 0)
            {
                _m_list.remove(i);
                _m_lTotalWeight -= wei.weight;

                return wei.value;
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
        WeightValue<T> item = new WeightValueList.WeightValue<T>();
        item.weight = weight;
        item.value = value;
        _m_lTotalWeight += item.weight;
        _m_list.add(item);

    }
    
    /**
     * 只增加新对象
     * @param value
     * @param weight
     */
    public void addCheckNew(T value, long weight)
    {
        WeightValue<T> item = lookup(value);
        if (null != item)
        	return;
        
        item = new WeightValue<T>();
        item.weight = weight;
        item.value = value;

        _m_list.add(item);

        _m_lTotalWeight += weight;
    }

    /**************************
     * 增加权重，需要计算原权重是否存在
     * @param value
     * @param weight
     */
    public void addAndCheck(T value, long weight)
    {
        WeightValue<T> item = lookup(value);
        if (null == item)
        {
            item = new WeightValue<T>();
            item.weight = weight;
            item.value = value;

            _m_list.add(item);
        } else
        {
            item.weight += weight;
        }

        _m_lTotalWeight += weight;
    }

    public List<WeightValue<T>> itemList()
    {
        return _m_list;
    }

    public void removeWeightByValue(T value)
    {
        for (WeightValue<T> item : _m_list)
        {
            if (item.value.equals(value))
            {
                _m_lTotalWeight -= item.weight;
                _m_list.remove(item);
                return;
            }
        }
    }

    /*****
     * 复制一份新的随机列表
     * @return
     */
    public WeightValueList<T> duplicate()
    {
        WeightValueList<T> ret = new WeightValueList<T>();
        for (WeightValue<T> tWeightValue : _m_list)
        {
            ret._m_list.add(tWeightValue.duplicate());
        }

        ret._m_lTotalWeight = _m_lTotalWeight;
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
        _m_lTotalWeight = 0;
    }

    /**
     * 修改权重
     * @param _percentUpgradePer 修正万分比，正值增加，负值减少
     */
    public void changeWei(long _percentUpgradePer)
    {
        //修改权重之前，要先保证顺序
        if (_percentUpgradePer > 0)
        {
            addWeightByPercent(_percentUpgradePer);
        } else
        {
            reduceWeightByPercent(-_percentUpgradePer);
        }
    }

    /**
     * 1.按万分比增加概率，每一项乘以（10000 + upgrade),
     * 2.将所有权重合计 - 10000 得出差值
     * 3.按列表顺序计算 （每一项概率 - 差值），如果减去差值后该项值小于0，按列表顺序重复该步骤
     * <p>
     * 1. Sn‘ = Sn * (10000+upgrade)
     * 2. R = 10000 - (Sn'+S(n-1)'....S0')
     * 3. S0'' = S0' - R;
     * R' = R - S0';
     * 若
     * R'>0
     * 则
     * S1'' = S1' - R'
     * 否则结束
     * @param _upgrade 增加概率
     */
    public void addWeightByPercent(long _upgrade)
    {
        long oldTotalWei = _m_lTotalWeight;
        long newTotalWei = 0;
        //等比放大
        for (WeightValue<T> weightValue : _m_list)
        {
            if (weightValue == null)
            {
                continue;
            }
            long newWei = weightValue.weight * (10000 + _upgrade) / 10000L;
            weightValue.weight = newWei;
            newTotalWei += newWei;
        }
        //需要再减回去的权重
        long changeWei = newTotalWei - oldTotalWei;
        for (WeightValue<T> weightValue : _m_list)
        {
            if (changeWei <= 0)
            {
                break;
            }
            if (weightValue == null)
            {
                continue;
            }
            //需要修正的权重
            long reduceWei = Math.min(weightValue.weight, changeWei);
            changeWei = changeWei - reduceWei;
            //修正后的权重
            weightValue.weight = weightValue.weight - reduceWei;
        }
    }

    /**
     * 1.按万分比增加概率，每一项乘以（10000 + _reduce),
     * 2.将所有权重合计 - 10000 得出差值
     * 3.按列表倒序计算 （每一项概率 - 差值），如果减去差值后该项值小于0，按列表顺序重复该步骤
     * 1. Sn‘ = Sn * (10000+_reduce)
     * 2. R = 10000 - (Sn'+S(n-1)'....S0')
     * 3. Sn'' = Sn' - R;
     * R' = R - Sn';
     * 若
     * R'>0
     * 则
     * S(n-1)'' = S(n-1)' - R'
     * 否则结束
     * @param _reduce 减少概率
     */
    public void reduceWeightByPercent(long _reduce)
    {
        long oldTotalWei = _m_lTotalWeight;
        long newTotalWei = 0;
        //等比放大
        for (WeightValue<T> weightValue : _m_list)
        {
            if (weightValue == null)
            {
                continue;
            }
            long newWei = weightValue.weight * (10000 + _reduce) / 10000L;
            weightValue.weight = newWei;
            newTotalWei += newWei;
        }
        //需要再加回去的权重
        long changeWei = newTotalWei - oldTotalWei;
        for (int i = _m_list.size() - 1; i >= 0; i--)
        {
            WeightValue<T> weightValue = _m_list.get(i);
            if (changeWei <= 0)
            {
                break;
            }
            if (weightValue == null)
            {
                continue;
            }
            //需要修正的权重
            long reduceWei = Math.min(weightValue.weight, changeWei);
            changeWei = changeWei - reduceWei;
            //修正后的权重
            weightValue.weight = weightValue.weight - reduceWei;
        }
    }

    @Override
    public String toString()
    {
        return StringFunc.list2String(_m_list);
    }

    public WeightValue<T> lookup(T _t)
    {
        for (WeightValue<T> weightValue : _m_list)
        {
            if (weightValue.value.equals(_t))
            {
                return weightValue;
            }
        }
        return null;
    }

    /**
     * 将给定权重列表与自己合并
     * @param _weiValueList 权重列表
     */
    public void merge(WeightValueList<T> _weiValueList)
    {
        if (_weiValueList == null)
        {
            return;
        }
        long totalWei = _m_lTotalWeight;
        for (WeightValue<T> weightValue : _weiValueList._m_list)
        {
            if (weightValue == null)
            {
                continue;
            }
            //查找自身的相同value的权重对象
            WeightValue<T> oldWeightValue = lookup(weightValue.value);
            if (oldWeightValue == null)
            {
                //没有则创建一个相同value的对象
                oldWeightValue = new WeightValue<>(weightValue.value, 0);
                _m_list.add(oldWeightValue);
            }
            //权重累计
            oldWeightValue.weight += weightValue.weight;
            totalWei += weightValue.weight;
        }
        _m_lTotalWeight = totalWei;

    }
}
