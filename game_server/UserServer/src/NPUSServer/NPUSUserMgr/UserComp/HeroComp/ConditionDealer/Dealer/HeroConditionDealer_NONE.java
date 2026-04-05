package NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EHeroConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer._AHeroConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

public class HeroConditionDealer_NONE extends _AHeroConditionDealer
{
    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.NONE;
    }

    @Override
    public boolean isEnable(_ABasicHeroCondition _cond, HeroInfo _data, NPVarInfo _varVariableInfo)
    {
        return false;
    }
}
