package NPGameRes.GameObjs.CommonObj.GroupValue.ParseObj;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class CommGroupValueItemList<T>
{
    private ArrayList<CommGroupValueItem<T>> _m_alItemObjList;

    public CommGroupValueItemList()
    {
        _m_alItemObjList = new ArrayList<>();
    }

    /**
     * 获取指定下标的一组数据
     * @param _idx
     * @return
     */
    public CommGroupValueItem<T> getItemObjList(int _idx)
    {
        if (_idx < 0 || _idx >= _m_alItemObjList.size())
            return null;

        return _m_alItemObjList.get(_idx);
    }

    /**
     * 获取随机的一组数据
     * @return
     */
    public CommGroupValueItem<T> getRandItemObjList()
    {
        if (_m_alItemObjList.size() <= 0)
            return null;

        int idx = CommonFunc.randomInt(_m_alItemObjList.size() - 1);
        return _m_alItemObjList.get(idx);
    }

    public ArrayList<CommGroupValueItem<T>> getAllItemObjList()
    {
        return _m_alItemObjList;
    }

    @SuppressWarnings("rawtypes")
    protected boolean parseStr(Class clazz, String sValue)
    {
        if (sValue.trim().isEmpty())
            return true;

        String[] itemS = CommonFunc.charSplit(sValue, ';');

        for (String itemDataS : itemS)
        {
            CommGroupValueItem<T> item = new CommGroupValueItem<>();
            if (!item.parseStr(clazz, itemDataS, ':'))
                return false;

            _m_alItemObjList.add(item);
        }

        return true;
    }
}
