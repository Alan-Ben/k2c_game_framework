package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.PlayerVariable_CS_CONSORT_RES_COUNT;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_CONSORT_RES_COUNT extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_CONSORT_RES_COUNT;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
    	PlayerVariable_CS_CONSORT_RES_COUNT obj = (PlayerVariable_CS_CONSORT_RES_COUNT) _variableObj;
        
    	ConsortInfo consort = _userData.getConsortComponent().lookup(obj.getConsortId());
    	if(null == consort)
    		return 0;
    	
    	return consort.getConsortRes(obj.getConsortResType());
    }
}
