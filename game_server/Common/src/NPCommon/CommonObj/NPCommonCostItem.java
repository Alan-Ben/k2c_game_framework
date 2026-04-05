package NPCommon.CommonObj;

import NPCommon.Log.CommLog;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.RefData._IParseFromStringable;
import NPEnum.ENPItemType;


public class NPCommonCostItem extends NPCommonItem implements _IParseFromStringable
{

    private long _m_lCount = 0;

    public NPCommonCostItem()
    {
    }

    public NPCommonCostItem(ENPItemType _itemType, long _itemId, long _itemCount)
    {
        super(_itemType, _itemId);
        _m_lCount = _itemCount;
    }

    public NPCommonCostItem(NPCommonItem _commItem, long _itemCount)
    {
        super(_commItem.getItemType(), _commItem.getItemId());
        _m_lCount = _itemCount;
    }

    public NPCommonCostItem(NPCommon_ItemInfo _item)
    {

        super(ENPItemType.ENPItemType_FromInt(_item.getItemType()), _item.getSubId());
        _m_lCount = _item.getCount();
    }

    public void addCount(long _addCount)
    {
        _m_lCount += _addCount;
    }

    public void setCount(long _count)
    {
        _m_lCount = _count;
    }

    public long getCount()
    {
        return _m_lCount;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue.trim().isEmpty())
            return true;
        NPStringReader stringReader = new NPStringReader(sValue);
        String sItemType = stringReader.readItem(':', '-');
        String sItemSubType = stringReader.readItem(':', '-');
        String sCount = stringReader.readItem();
        if (sItemType == null || sItemType.isEmpty())
        {
            CommLog.error("物品解析失败：str ={}", sValue, new Exception(""));
            return false;
        }
        if (sItemSubType == null || sItemSubType.isEmpty())
        {
            CommLog.error("物品解析失败：str ={}", sValue, new Exception(""));
            return false;
        }
        if (sCount == null || sCount.isEmpty())
        {
            CommLog.error("物品解析失败：str ={}", sValue, new Exception(""));
            return false;
        }
        try
        {

            this._m_eItemType = ENPItemType.valueOf(sItemType.toUpperCase());
            this._m_lItemId = Long.parseLong(sItemSubType.trim());
            this._m_lCount = Long.parseLong(sCount.trim());
        } catch (Exception e)
        {
            CommLog.error("物品解析失败：str ={}", sValue, e);
            return false;
        }
        return true;
    }

    public NPCommonCostItem duplicate()
    {
        return new NPCommonCostItem(_m_eItemType, _m_lItemId, _m_lCount);
    }

    public NPCommonCostItem multi(long _multi)
    {
        return new NPCommonCostItem(_m_eItemType, _m_lItemId, _m_lCount * _multi);
    }

    /****
     * 数量乘以万分比
     * @param _item
     * @param _percent
     * @return
     */
    public static NPCommonCostItem mulPercent(NPCommonCostItem _item, long _percent)
    {
        return new NPCommonCostItem(_item._m_eItemType, _item._m_lItemId, _item._m_lCount * _percent / 10000L);
    }

    /**
     * 生成协议里使用的对象
     * @return
     */
    public NPCommon_ItemInfo toProto()
    {
        NPCommon_ItemInfo proto = new NPCommon_ItemInfo(getItemType().ordinal(), getItemId(), getCount(), null);
        return proto;
    }

    @Override
    public String toString()
    {
        return String.format("%s-%d:%d", getItemType(), getItemId(), getCount());
    }

    /*****
     * 物品数量为空
     * @return
     */
    public boolean isEmpty()
    {
        return _m_lCount == 0;
    }
}
