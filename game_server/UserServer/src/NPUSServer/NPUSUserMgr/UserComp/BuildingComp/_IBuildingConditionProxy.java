package NPUSServer.NPUSUserMgr.UserComp.BuildingComp;

import CommonEnum.ESpecAttrType;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPConditionDealerData;

public interface _IBuildingConditionProxy extends _ITNPConditionDealerData
{
    long getBuildingId();

    ESpecAttrType getAttrType();
}
