package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

public class NPPlayerCondition_NONE extends _ANPBasicPlayerCondition
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.NONE;
    }

    public static NPPlayerCondition_NONE readCond(String _str)
    {
        NPPlayerCondition_NONE cond = new NPPlayerCondition_NONE();

        return cond;
    }
}
