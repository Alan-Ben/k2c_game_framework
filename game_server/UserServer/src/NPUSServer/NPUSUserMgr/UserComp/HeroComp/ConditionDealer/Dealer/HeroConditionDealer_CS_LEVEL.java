package NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EHeroConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.HeroCondition.ConditionObj.HeroCondition_CS_LEVEL;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.HeroConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer._AHeroConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

public class HeroConditionDealer_CS_LEVEL extends _AHeroConditionDealer
{
    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.CS_LEVEL;
    }

    @Override
    public boolean isEnable(_ABasicHeroCondition _cond, HeroInfo _heroInfo, NPVarInfo _varVariableInfo)
    {
        HeroCondition_CS_LEVEL cond = (HeroCondition_CS_LEVEL) _cond;
        return HeroConditionDealerMgr.isRange(_heroInfo.getLevel(), cond.minValue(), cond.maxValue());
    }
}
