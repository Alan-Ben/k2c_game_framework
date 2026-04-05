package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 是否解锁旅店菜品 CS_HAD_UNLOCK_INN_DISH:菜品id
 * @author
 */
public class NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH extends _ANPBasicPlayerCondition
{
    private long _m_dishId;

    public long getDishId()
    {
        return _m_dishId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAD_UNLOCK_INN_DISH;
    }

    public static NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH cond = new NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH();

        String rawDishId = _reader.readItem(':');
        if (null == rawDishId)
        {
            CommLog.error("CS_HAD_UNLOCK_INN_DISH condition read error: missing dishId");
            return null;
        }
        cond._m_dishId = Long.parseLong(rawDishId);

        return cond;
    }
}
