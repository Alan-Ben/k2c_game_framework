package NPCommon.Game;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.List;

public class WeightIndexList implements _IParseFromStringable
{
    private WeightValueList<Integer> _m_list = new WeightValueList<>();

    @Override
    public boolean parseFromString(String _str)
    {
        List<Integer> weightList = CommonFunc.listIntFromString(_str);
        for (int i = 0; i < weightList.size(); i++)
        {
            Integer value = weightList.get(i);
            if (value == null)
                continue;

            _m_list.add(i, value); // 默认权重为1
        }
        return true;
    }

    public WeightValueList<Integer> getList()
    {
        return _m_list;
    }
}
