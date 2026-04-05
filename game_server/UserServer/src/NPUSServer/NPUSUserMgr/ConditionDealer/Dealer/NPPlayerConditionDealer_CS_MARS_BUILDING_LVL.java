package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_MARS_BUILDING_LVL;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;

public class NPPlayerConditionDealer_CS_MARS_BUILDING_LVL extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_MARS_BUILDING_LVL;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_MARS_BUILDING_LVL cond = (NPPlayerCondition_CS_MARS_BUILDING_LVL) _cond;

        MarsBuildingInfo building = _userData.getMarsBuildingComponent().lookupBuilding(cond.id());
        if(null == building)
        	return false;
        
        long value = building.getBuildingLvl();
        return NPPlayerConditionDealerMgr.isRange(value, cond.minValue(), cond.maxValue());
    }
}
