package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_BUILDING_FUNC_LEVEL;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;

public class NPPlayerConditionDealer_CS_BUILDING_FUNC_LEVEL extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_BUILDING_FUNC_LEVEL;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_BUILDING_FUNC_LEVEL cond = (NPPlayerCondition_CS_BUILDING_FUNC_LEVEL) _cond;

        BuildingInfo buildingInfo = _userData.getBuildingComponent().lookupBuilding(cond.buildingId());
        if (buildingInfo == null)
            return false;

        return NPPlayerConditionDealerMgr.isRange(buildingInfo.getBuildingFuncLevel(cond.getFuncType()), cond.minLvl(), cond.maxLvl());
    }
}
