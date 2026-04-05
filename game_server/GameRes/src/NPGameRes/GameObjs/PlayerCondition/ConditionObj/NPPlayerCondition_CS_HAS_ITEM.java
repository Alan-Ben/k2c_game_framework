package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

public class NPPlayerCondition_CS_HAS_ITEM extends _ANPBasicPlayerCondition
{
    private ENPItemType _m_eItemType;//物品类型
    private long _m_lItemId;//物品子ID
    private long _m_lMinValue = -1; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public ENPItemType itemType()
    {
        return _m_eItemType;
    }

    public long itemId()
    {
        return _m_lItemId;
    }

    public long minValue()
    {
        return _m_lMinValue;
    }

    public long maxValue()
    {
        return _m_lMaxValue;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAS_ITEM;
    }

    public static NPPlayerCondition_CS_HAS_ITEM readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_HAS_ITEM cond = new NPPlayerCondition_CS_HAS_ITEM();

        String itemTypeS = _reader.readItem('-');
        String itemIdS = _reader.readItem(':');
        if (null == itemTypeS || null == itemIdS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_HAS_ITEM[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_eItemType = ENPItemType.valueOf(itemTypeS.toUpperCase());
        cond._m_lItemId = Long.parseLong(itemIdS);

        String minVS = _reader.readItem();
        if (null != minVS)
            cond._m_lMinValue = Long.parseLong(minVS);
        else
            cond._m_lMinValue = 1;

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}
