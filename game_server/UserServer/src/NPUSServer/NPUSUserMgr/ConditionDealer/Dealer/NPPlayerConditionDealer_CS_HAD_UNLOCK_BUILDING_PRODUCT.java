package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;

public class NPPlayerConditionDealer_CS_HAD_UNLOCK_BUILDING_PRODUCT extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAD_UNLOCK_BUILDING_PRODUCT;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT cond = (NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT) _cond;

        BuildingInfo buildingInfo = _userData.getBuildingComponent().lookupBuilding(cond.getBuildingId());
        if (buildingInfo == null)
            return false;

        BuildingBusinessFunc business = buildingInfo.getBusiness();
        if (business == null)
            return false;

        return business.hasUnlockProduct(cond.getProductId());
    }
}
