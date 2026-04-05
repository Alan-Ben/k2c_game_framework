package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_VALUE_ID;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_VALUE_ID extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_VALUE_ID;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_VALUE_ID obj = (NPPlayerVariable_CS_VALUE_ID) _variableObj;

        switch (obj.getValueType())
        {
            case CONSORT_INTIMACY:
            {
                ConsortInfo consortInfo = _userData.getConsortComponent().lookup(obj.getId());
                if (consortInfo == null)
                    return 0;

                return consortInfo.getIntimacy();
            }
            case INN_STATION_LEVEL:
                return _userData.getInnComponent().getStationMgr().getStationLevel(obj.getId());
            case BUILDING_EARNINGS:
            {
                BuildingInfo buildingInfo = _userData.getBuildingComponent().lookupBuilding(obj.getId());
                if (buildingInfo == null)
                    return 0;
                BuildingBusinessFunc business = buildingInfo.getBusiness();
                if (business == null)
                    return 0;
                return business.getSpeed();
            }
            default:
                return 0L;
        }
    }
}
