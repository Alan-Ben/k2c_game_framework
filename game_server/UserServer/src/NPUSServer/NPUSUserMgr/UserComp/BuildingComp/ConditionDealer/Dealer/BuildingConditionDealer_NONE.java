package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.Dealer;

import Common.ConditionEnum.EBuildingConditionType;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer._ABuildingConditionDealer;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp._IBuildingConditionProxy;

public class BuildingConditionDealer_NONE extends _ABuildingConditionDealer
{
    @Override
    public EBuildingConditionType conditionType()
    {
        return EBuildingConditionType.NONE;
    }

    @Override
    public boolean isEnable(_ABasicBuildingCondition _cond, _IBuildingConditionProxy _data, NPVarInfo _varVariableInfo)
    {
        return false;
    }
}
