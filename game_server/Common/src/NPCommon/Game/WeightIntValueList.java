package NPCommon.Game;

import NPCommon.Game.WeightValueList.WeightValue;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.List;

public class WeightIntValueList implements _IParseFromStringable
{
    private WeightValueList<Integer> _m_list = new WeightValueList<>();

    public WeightIntValueList()
    {
    }
    /**
     * 给定值列表和权重列表构造对象
     * @param _valueList  值列表
     * @param _weightList 权重列表
     */
    public WeightIntValueList(List<Integer> _valueList, List<Integer> _weightList)
    {
        for (int i = 0; i < _valueList.size(); i++)
        {
            Integer value = _valueList.get(i);
            if (value == null)
            {
                continue;
            }
            Integer weight = _weightList.get(i);
            if (null == weight)
            {
                continue;
            }
            _m_list.add(value, weight);
        }
    }
    
    public WeightValueList<Integer> getList() {return _m_list;}

    @Override
    public boolean parseFromString(String sValue)
    {
        return parseFromString(sValue, ';', ':');
    }

    public boolean parseFromString(String sValue, char token1, char token2)
    {
        if (sValue == null || sValue.trim().isEmpty()) return true;

        String[] steps = CommonFunc.charSplit(sValue, token1);
        for (String str : steps)
        {
            String[] infos = CommonFunc.charSplit(str, token2);
            if (infos.length == 2)
            {
                int value = Integer.parseInt(infos[0].trim());
                int weight = Integer.parseInt(infos[1].trim());
                _m_list.add(value, weight);
            } else
            {
                CommLog.error("WeightValueList 参数数量不为2 str:{} ", sValue, new Exception());
                return false;
            }
        }
        return true;
    }

    public WeightIntValueList clone()
    {
        WeightIntValueList ret = new WeightIntValueList();
        for (WeightValue<Integer> wightValue : _m_list.itemList())
        {
            ret.add(wightValue.value, wightValue.weight);
        }
        return ret;
    }

    public int random()
    {
        return _m_list.random();
    }

    public int randomAndRemove()
    {
        return _m_list.randomAndRemove();
    }

    public boolean isEmpty()
    {
        return _m_list.isEmpty();
    }

    public void add(int value, long weight)
    {
        _m_list.add(value, weight);
    }

    public void removeWeightByValue(int value)
    {
        _m_list.removeWeightByValue(value);
    }


}
