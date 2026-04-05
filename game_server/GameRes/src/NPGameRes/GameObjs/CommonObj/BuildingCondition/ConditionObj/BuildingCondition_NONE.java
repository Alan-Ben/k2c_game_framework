package NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj;

import Common.ConditionEnum.EBuildingConditionType;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;

public class BuildingCondition_NONE extends _ABasicBuildingCondition
{
    @Override
    public EBuildingConditionType conditionType()
    {
        return EBuildingConditionType.NONE;
    }

    public static BuildingCondition_NONE readCond(String _str)
    {
        return new BuildingCondition_NONE();
    }
}
