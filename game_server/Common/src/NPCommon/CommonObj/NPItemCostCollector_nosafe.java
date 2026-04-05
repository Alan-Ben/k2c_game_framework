package NPCommon.CommonObj;

import NPCommon.NPCommon_ItemInfo;
import NPEnum.ENPItemType;

import java.util.ArrayList;
import java.util.List;

/*************
 * 消耗的收集器，这边所有消耗会被整合到一起
 *
 * 注意本收集器不要在不同任务内使用，需要拷贝使用。是线程不安全结构体！！！
 * @author mj
 *
 */
public class NPItemCostCollector_nosafe
{
    // 奖励列表
    private List<NPCommonCostItem> _m_ItemList;

    public NPItemCostCollector_nosafe()
    {
        _m_ItemList = new ArrayList<>();
    }

    /******
     * 返回可堆叠物品列表
     * @return
     */
    public List<NPCommonCostItem> getItemList()
    {
        return _m_ItemList;
    }

    /******
     * 返回可堆叠物品列表
     * @return
     */
    public List<NPCommon_ItemInfo> getItemListP()
    {
        List<NPCommon_ItemInfo> list = new ArrayList<>();
        for (NPCommonCostItem item : _m_ItemList)
        {
            list.add(new NPCommon_ItemInfo(item.getItemType().ordinal(), item.getItemId(), item.getCount(), null));
        }
        return list;
    }

    /**
     * 返回倍乘物品列表
     * @param _multiple 倍乘系数
     * @return 物品列表
     */
    public List<NPCommonCostItem> getItemListWithMultiple(int _multiple)
    {
        List<NPCommonCostItem> itemList = new ArrayList<>();
        for (NPCommonCostItem item : _m_ItemList)
        {
            itemList.add(new NPCommonCostItem(item.getItemType(), item.getItemId(), item.getCount() * _multiple));
        }
        return itemList;
    }

    /**************************
     * 奖励是否为空
     *
     * @return
     */
    public boolean isEmpty()
    {
        return _m_ItemList.size() == 0;
    }

    private NPCommonCostItem ensureItem(ENPItemType _eItemType, long _subId)
    {
        NPCommonCostItem item = lookupItem(_eItemType, _subId);
        if (null != item)
        {
            return item;
        } else
        {
            item = new NPCommonCostItem(_eItemType, _subId, 0);
            _m_ItemList.add(item);
            return item;
        }
    }

    public NPCommonCostItem lookupItem(ENPItemType _eItemType, long _subId)
    {
        for (NPCommonCostItem _item : _m_ItemList)
        {
            if (_item.getItemType() == _eItemType && _item.getItemId() == _subId)
            {
                return _item;
            }
        }

        return null;
    }

    public void addItemList(List<NPCommonCostItem> _itemList)
    {
        for (NPCommonCostItem npCommonCostItem : _itemList)
        {
            addItem(npCommonCostItem.getItemType(), npCommonCostItem.getItemId(), npCommonCostItem.getCount());
        }
    }

    public void addCostItemList(List<NPCommonCostItem> _itemList)
    {
        for (NPCommonCostItem npCommonCostItem : _itemList)
        {
            addCostItem(npCommonCostItem);
        }
    }

    /**************************
     * 增加一个物品By子表
     *
     * @param _eItemType
     * @param _subId
     * @param _num
     */
    public void addItem(ENPItemType _eItemType, long _subId, long _num)
    {
        NPCommonCostItem itemInfo = ensureItem(_eItemType, _subId);
        itemInfo.setCount(itemInfo.getCount() + _num);
    }

    public void addItem(NPCommonCostItem item)
    {
        addItem(item.getItemType(), item.getItemId(), item.getCount());
    }

    /*********
     * 只添加需要扣除的物品
     * @param item
     */
    public void addCostItem(NPCommonCostItem item)
    {
        if (null == item || ENPItemType.NONE == item.getItemType() || item.getCount() <= 0)
            return;

        addItem(item.getItemType(), item.getItemId(), item.getCount());
    }

    /**
     * 增加需要倍乘的物品列表
     * @param _itemList      物品列表
     * @param _multipleTimes 需要翻倍的倍数
     */
    public void addMultipleItemList(List<NPCommonCostItem> _itemList, int _multipleTimes)
    {
        for (NPCommonCostItem item : _itemList)
        {
            addMultipleItem(item, _multipleTimes);
        }
    }

    /**
     * 增加需要倍乘的物品
     * @param _item          物品
     * @param _multipleTimes 需要翻倍的倍数
     */
    public void addMultipleItem(NPCommonCostItem _item, int _multipleTimes)
    {
        addItem(_item.getItemType(), _item.getItemId(), _item.getCount() * _multipleTimes);
    }

    /**************************
     * 移除单个物品，只移除可堆叠物品
     * @param _eItemType
     * @param _subId
     * @param _num
     */
    public void removeItem(ENPItemType _eItemType, long _subId, long _num)
    {
        NPCommonCostItem itemInfo = lookupItem(_eItemType, _subId);
        if (null != itemInfo)
        {
            long leftCount = Math.max(0, itemInfo.getCount() - _num); //物品数量最多减少到0
            itemInfo.setCount(leftCount);
        }
    }

    public void removeItem(NPCommonCostItem item)
    {
        removeItem(item.getItemType(), item.getItemId(), item.getCount());
    }

    /****
     * 指定物品列表进行移除，只针对可堆叠物品
     * @param _itemList
     */
    public void removeItemList(List<NPCommonCostItem> _itemList)
    {
        for (NPCommonCostItem item : _itemList)
        {
            removeItem(item);
        }
    }

    public void addItemP(NPCommon_ItemInfo item)
    {
        addItem(ENPItemType.ENPItemType_FromInt(item.getItemType()), item.getSubId(), item.getCount());
    }

    public void addItemListP(List<NPCommon_ItemInfo> _itemList)
    {
        for (NPCommon_ItemInfo item : _itemList)
        {
            addItemP(item);
        }
    }
}
