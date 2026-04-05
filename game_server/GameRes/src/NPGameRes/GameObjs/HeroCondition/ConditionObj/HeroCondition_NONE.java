package NPGameRes.GameObjs.HeroCondition.ConditionObj;

import Common.ConditionEnum.EHeroConditionType;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;

public class HeroCondition_NONE extends _ABasicHeroCondition
{
    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.NONE;
    }

    public static HeroCondition_NONE readCond(String _str)
    {
        return new HeroCondition_NONE();
    }
}
