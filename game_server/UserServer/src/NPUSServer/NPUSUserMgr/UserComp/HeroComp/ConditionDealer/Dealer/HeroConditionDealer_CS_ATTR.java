package NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EHeroConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.HeroCondition.ConditionObj.HeroCondition_CS_ATTR;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer._AHeroConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

public class HeroConditionDealer_CS_ATTR extends _AHeroConditionDealer
{
    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.CS_ATTR;
    }

    @Override
    public boolean isEnable(_ABasicHeroCondition _cond, HeroInfo _heroInfo, NPVarInfo _varVariableInfo)
    {
        HeroCondition_CS_ATTR cond = (HeroCondition_CS_ATTR) _cond;
        return _heroInfo.getRef().spec_attr_type == cond.attrType();
    }
}
