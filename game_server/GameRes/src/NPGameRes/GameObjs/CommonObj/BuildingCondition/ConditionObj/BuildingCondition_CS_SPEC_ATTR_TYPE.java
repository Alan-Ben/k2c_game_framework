package NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj;

import Common.ConditionEnum.EBuildingConditionType;
import CommonEnum.ESpecAttrType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;

public class BuildingCondition_CS_SPEC_ATTR_TYPE extends _ABasicBuildingCondition
{
    private ESpecAttrType _m_attrType = ESpecAttrType.NONE;

    public ESpecAttrType getAttrType()
    {
        return _m_attrType;
    }

    @Override
    public EBuildingConditionType conditionType()
    {
        return EBuildingConditionType.CS_SPEC_ATTR_TYPE;
    }

    public static BuildingCondition_CS_SPEC_ATTR_TYPE readCond(NPStringReader _reader)
    {
        BuildingCondition_CS_SPEC_ATTR_TYPE cond = new BuildingCondition_CS_SPEC_ATTR_TYPE();

        String rawType = _reader.readItem();
        if (null == rawType)
        {
            CommLog.error("Can not read str for EBuildingConditionType.CS_SPEC_ATTR_TYPE[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_attrType = ESpecAttrType.valueOf(rawType.toUpperCase());

        return cond;
    }
}
