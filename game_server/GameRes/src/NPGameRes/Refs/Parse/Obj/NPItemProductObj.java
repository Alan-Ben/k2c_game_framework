package NPGameRes.Refs.Parse.Obj;

import NPEnum.ENPItemType;

public class NPItemProductObj
{
    private ENPItemType _m_eItemType;
    private long _m_lItemId;
    private int _m_iBasicCount; //产出基数

    public NPItemProductObj(ENPItemType _itemType, long _itemId, int _basicCount)
    {
        _m_eItemType = _itemType;
        _m_lItemId = _itemId;
        _m_iBasicCount = _basicCount;
    }

    public ENPItemType getItemType()
    {
        return _m_eItemType;
    }

    public long getItemId()
    {
        return _m_lItemId;
    }

    public int getBasicCount()
    {
        return _m_iBasicCount;
    }
}
