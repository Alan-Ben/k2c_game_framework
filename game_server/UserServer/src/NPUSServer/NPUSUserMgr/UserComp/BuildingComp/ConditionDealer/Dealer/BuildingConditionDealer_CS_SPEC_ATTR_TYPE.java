package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EBuildingConditionType;
import CommonEnum.ESpecAttrType;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj.BuildingCondition_CS_SPEC_ATTR_TYPE;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer._ABuildingConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp._IBuildingConditionProxy;

public class BuildingConditionDealer_CS_SPEC_ATTR_TYPE extends _ABuildingConditionDealer
{
    @Override
    public EBuildingConditionType conditionType()
    {
        return EBuildingConditionType.CS_SPEC_ATTR_TYPE;
    }

    @Override
    public boolean isEnable(_ABasicBuildingCondition _cond, _IBuildingConditionProxy _buildingInfo, NPVarInfo _varVariableInfo)
    {
        BuildingCondition_CS_SPEC_ATTR_TYPE cond = (BuildingCondition_CS_SPEC_ATTR_TYPE) _cond;

        ESpecAttrType attrType = _buildingInfo.getAttrType();
        if (attrType == null)
            return false;

        return attrType == cond.getAttrType();
    }
}
