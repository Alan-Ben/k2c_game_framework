package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EBuildingConditionType;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj.BuildingCondition_CS_BUILDING_ID;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer._ABuildingConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp._IBuildingConditionProxy;

public class BuildingConditionDealer_CS_BUILDING_ID extends _ABuildingConditionDealer
{
    @Override
    public EBuildingConditionType conditionType()
    {
        return EBuildingConditionType.CS_BUILDING_ID;
    }

    @Override
    public boolean isEnable(_ABasicBuildingCondition _cond, _IBuildingConditionProxy _buildingInfo, NPVarInfo _varVariableInfo)
    {
        BuildingCondition_CS_BUILDING_ID cond = (BuildingCondition_CS_BUILDING_ID) _cond;
        return _buildingInfo.getBuildingId() == cond.getBuildingId();
    }
}
