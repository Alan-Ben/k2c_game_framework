package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_MARS_TECH_LVL;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;

public class NPPlayerConditionDealer_CS_MARS_TECH_LVL extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_MARS_TECH_LVL;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_MARS_TECH_LVL cond = (NPPlayerCondition_CS_MARS_TECH_LVL) _cond;

        MarsTechInfo info = _userData.getMarsTechComponent().lookup(cond.id());
        if(null == info)
        	return false;
        
        long value = info.getLvl();
        return NPPlayerConditionDealerMgr.isRange(value, cond.minValue(), cond.maxValue());
    }
}
