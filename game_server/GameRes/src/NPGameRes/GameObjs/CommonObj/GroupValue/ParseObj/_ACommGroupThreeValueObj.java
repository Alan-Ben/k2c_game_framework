package NPGameRes.GameObjs.CommonObj.GroupValue.ParseObj;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.lang.reflect.ParameterizedType;
import java.util.ArrayList;

/*****
 * 解析三层数据
 * 例：2000:4000;4001:7000;7001:8000;8001:9000;9001:10000|2000:4000;4001:7000;7001:8000;8001:9000;9001:10000
 *
 * @author mj
 *
 * @param <T>
 */
public abstract class _ACommGroupThreeValueObj<T> implements _IParseFromStringable
{
    private ArrayList<CommGroupValueItemList<T>> _m_alItemListObjList;

    public _ACommGroupThreeValueObj()
    {
        _m_alItemListObjList = new ArrayList<>();
    }

    public ArrayList<CommGroupValueItemList<T>> getAllItemListObj()
    {
        return _m_alItemListObjList;
    }

    public CommGroupValueItemList<T> getItemListObj(int _idx)
    {
        if (_idx < 0 || _idx >= _m_alItemListObjList.size())
            return null;

        return _m_alItemListObjList.get(_idx);
    }

    /**
     * 获取最终随机一组数据
     * @param _idx
     * @return
     */
    public T getRandItem(int _idx)
    {
        CommGroupValueItemList<T> itemListObj = getItemListObj(_idx);
        if (null == itemListObj)
            return null;

        CommGroupValueItem<T> itemList = itemListObj.getItemObjList(_idx);
        if (null == itemList)
            return null;

        return itemList.getRandItem();
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
            CommGroupValueItemList<T> listObj = new CommGroupValueItemList<>();
            if (!listObj.parseStr(__getGenericClassT(), itemListDataS))
                return false;

            _m_alItemListObjList.add(listObj);
        }

        return true;
    }
}
