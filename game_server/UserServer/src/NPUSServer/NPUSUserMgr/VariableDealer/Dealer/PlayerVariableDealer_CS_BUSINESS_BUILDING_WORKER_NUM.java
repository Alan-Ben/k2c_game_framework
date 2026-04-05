package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_BUSINESS_BUILDING_WORKER_NUM extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_BUSINESS_BUILDING_WORKER_NUM;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
    	PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM obj = (PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM) _variableObj;
    	
    	return _userData.getBuildingComponent().getWorkerNum(obj.buildingId());
    }
}
