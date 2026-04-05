package NPCommon.CommonObj;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;


public class NPCommonItem implements _IParseFromStringable
{
    // 统一物品ID
    protected ENPItemType _m_eItemType = ENPItemType.NONE;
    protected long _m_lItemId = 10001;

    public NPCommonItem()
    {

    }

    public NPCommonItem(ENPItemType _itemType, long _itemId)
    {
        _m_eItemType = _itemType == null ? ENPItemType.NONE : _itemType;
        _m_lItemId = _itemId;
    }

    /*******
     * 生成唯一id
     */
    public static long makeUniqueId(int _itemType, long _itemId)
    {
        return _itemType + _itemId * 1000L;
    }

    public long getUniqueId()
    {
        return makeUniqueId(_m_eItemType.ordinal(), _m_lItemId);
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        String[] subStrs = CommonFunc.charSplit(sValue, new char[]{':', '-'});
        if (subStrs.length < 2)
        {
            CommLog.error("物品解析失败：str ={}", sValue, new Exception(""));
            return false;
        }
        this._m_eItemType = ENPItemType.valueOf(subStrs[0].trim().toUpperCase());
        this._m_lItemId = Long.parseLong(subStrs[1].trim());
        return true;
    }

    public NPCommonItem duplicate()
    {
        NPCommonItem obj = new NPCommonItem();
        obj._m_eItemType = _m_eItemType;
        obj._m_lItemId = _m_lItemId;
        return obj;
    }

    public ENPItemType getItemType()
    {
        return _m_eItemType;
    }

    public long getItemId()
    {
        return _m_lItemId;
    }

    @Override
    public String toString()
    {
        return String.format("%s-%d", getItemType(), getItemId());
    }
}
