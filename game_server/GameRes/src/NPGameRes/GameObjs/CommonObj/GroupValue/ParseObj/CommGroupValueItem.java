package NPGameRes.GameObjs.CommonObj.GroupValue.ParseObj;

import NPCommon.RefData.AbstractRefDataMgr;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class CommGroupValueItem<T>
{
    private ArrayList<T> _m_alItemList;

    public CommGroupValueItem()
    {
        _m_alItemList = new ArrayList<>();
    }

    public T getRandItem()
    {
        if (_m_alItemList.size() <= 0)
            return null;

        int idx = CommonFunc.randomInt(_m_alItemList.size() - 1);
        return _m_alItemList.get(idx);
    }

    public ArrayList<T> getItemList()
    {
        return _m_alItemList;
    }

    @SuppressWarnings({"rawtypes", "unchecked"})
    protected boolean parseStr(Class clazz, String sValue, char token)
    {
        if (sValue.trim().isEmpty())
            return true;

        String[] itemS = CommonFunc.charSplit(sValue, token);

        for (String itemDataS : itemS)
        {
            T value = (T) AbstractRefDataMgr.parseCreateObj(clazz, itemDataS, "", "CommGroupValueItem");
            if (null == value)
                return false;

            _m_alItemList.add(value);
        }

        return true;
    }
}
