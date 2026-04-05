package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 是否解锁建筑产品 CS_HAD_UNLOCK_BUILDING_PRODUCT:建筑id:产品id
 * @author
 */
public class NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT extends _ANPBasicPlayerCondition
{
    private long _m_buildingId;
    private long _m_productId;


    public long getBuildingId()
    {
        return _m_buildingId;
    }

    public long getProductId()
    {
        return _m_productId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAD_UNLOCK_BUILDING_PRODUCT;
    }

    public static NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT cond = new NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT();

        String rawBuildingId = _reader.readItem(':');
        if (null == rawBuildingId)
        {
            CommLog.error("NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT: Invalid format, missing building id.");
            return null;
        }
        cond._m_buildingId = Long.parseLong(rawBuildingId);

        String rawProductId = _reader.readItem(':');
        if (null == rawProductId)
        {
            CommLog.error("NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT: Invalid format, missing product id.");
            return null;
        }
        cond._m_productId = Long.parseLong(rawProductId);

        return cond;
    }
}
