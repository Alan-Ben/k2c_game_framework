package NPCommon.CommonObj;

import NPEnum.ENPItemType;

import java.util.ArrayList;

public class NPItemGroupObj
{
    private ArrayList<NPCommonCostItem> _m_alItemObjList;

    public NPItemGroupObj()
    {
        _m_alItemObjList = new ArrayList<NPCommonCostItem>();
    }

    public ArrayList<NPCommonCostItem> getItemObjList()
    {
        return _m_alItemObjList;
    }

    /**
     * 获取物品对象
     * @param _itemType
     * @param _itemId
     * @return
     */
    public NPCommonCostItem getItemObj(ENPItemType _itemType, long _itemId)
    {
        for (int i = 0; i < _m_alItemObjList.size(); i++)
        {
            NPCommonCostItem obj = _m_alItemObjList.get(i);
            if (null == obj)
                continue;

            if (obj._m_eItemType == _itemType && obj._m_lItemId == _itemId)
                return obj;
        }

        return null;
    }

    /**
     * 累计物品对象
     * @param _itemType
     * @param _itemId
     * @param _itemCount
     */
    public void collectItemObj(ENPItemType _itemType, long _itemId, long _itemCount)
    {
        NPCommonCostItem obj = getItemObj(_itemType, _itemId);
        if (null == obj)
        {
            obj = new NPCommonCostItem(_itemType, _itemId, _itemCount);
            _m_alItemObjList.add(obj);
        } else
        {
            obj.addCount(_itemCount);
        }
    }
}
