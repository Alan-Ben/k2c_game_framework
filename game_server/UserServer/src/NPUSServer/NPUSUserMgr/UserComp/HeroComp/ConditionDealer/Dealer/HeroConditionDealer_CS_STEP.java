package NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EHeroConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.HeroCondition.ConditionObj.HeroCondition_CS_STEP;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.HeroConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer._AHeroConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

public class HeroConditionDealer_CS_STEP extends _AHeroConditionDealer
{
    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.CS_STEP;
    }

    @Override
    public boolean isEnable(_ABasicHeroCondition _cond, HeroInfo _heroInfo, NPVarInfo _varVariableInfo)
    {
        HeroCondition_CS_STEP cond = (HeroCondition_CS_STEP) _cond;
        return HeroConditionDealerMgr.isRange(_heroInfo.getStep(), cond.minValue(), cond.maxValue());
    }
}
