package NPGameRes.GameObjs.CommonObj.GroupValue.ParseObj;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.lang.reflect.ParameterizedType;
import java.util.ArrayList;

/******
 * 解析2层数据
 * 例：30;30;20;15;5|30;30;20;15;5|30;30;20;15;5
 *
 * @author mj
 *
 * @param <T>
 */
public abstract class _ACommGroupTwoValueObj<T> implements _IParseFromStringable
{
    private ArrayList<CommGroupValueItem<T>> _m_alItemObjList;

    public _ACommGroupTwoValueObj()
    {
        _m_alItemObjList = new ArrayList<>();
    }

    /**
     * 获取指定一组数据
     * @param _idx
     * @return
     */
    public ArrayList<T> getItemList(int _idx)
    {
        if (_idx < 0 || _idx >= _m_alItemObjList.size())
            return null;

        CommGroupValueItem<T> itemListObj = _m_alItemObjList.get(_idx);
        if (null == itemListObj)
            return null;

        return itemListObj.getItemList();
    }

    /**
     * 获取泛型的Class对象
     * @return
     */
    @SuppressWarnings("unchecked")
    private Class<T> __getGenericClassT()
    {
        ParameterizedType parameterizedType = (ParameterizedType) getClass().getGenericSuperclass();
        return (Class<T>) parameterizedType.getActualTypeArguments()[0];
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue.trim().isEmpty())
            return true;

        String[] itemListS = CommonFunc.charSplit(sValue, '|');

        for (String itemListDataS : itemListS)
        {
            CommGroupValueItem<T> listObj = new CommGroupValueItem<>();
            if (!listObj.parseStr(__getGenericClassT(), itemListDataS, ';'))
                return false;

            _m_alItemObjList.add(listObj);
        }

        return true;
    }
}
