package NPCommon.Util;

import NPCommon.RefData.AbstractRefDataMgr;

import java.util.ArrayList;
import java.util.List;

/*******
 * 列表嵌套列表，第一层列表使用'|',第二层使用';'
 * @param <T>
 */
public class ListList<T>
{
    private List<List<T>> _m_listList = new ArrayList<>();

    @SuppressWarnings("unchecked")
    public boolean fromString(String _sValue, Class<?> _clazz)
    {
        _m_listList.clear();
        if (_sValue == null || _sValue.isEmpty())
            return false;
        List<String> strList = StringFunc.listStringFromString(_sValue, '|');
        for (String line : strList)
        {
            List<T> list = new ArrayList<>();
            List<String> strObjList = StringFunc.listStringFromString(line);
            for (String str : strObjList)
            {
                Object obj = AbstractRefDataMgr.parseCreateObj(_clazz, str, "", _clazz.getSimpleName());
                if (null != obj)
                    list.add((T) obj);
            }
            _m_listList.add(list);
        }
        return true;
    }

    public int size()
    {
        return _m_listList.size();
    }

    public List<T> get(int _index)
    {
        if (_index < 0 || _index >= _m_listList.size())
            return null;
        return _m_listList.get(_index);
    }

    public boolean isEmpty()
    {
        return _m_listList.isEmpty();
    }

    @Override
    public String toString()
    {
        return StringFunc.list2String(_m_listList, '|', new ClassFormatter<List<T>>()
        {
            @Override
            public String getString(List<T> _obj)
            {
                return StringFunc.list2String(_obj);
            }
        });
    }
}
