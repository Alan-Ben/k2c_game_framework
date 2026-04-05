package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_VAR_V;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_VAR_V extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_VAR_V;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_VAR_V variableObj = (NPPlayerVariable_CS_VAR_V) _variableObj;
        if (_variableInfo == null)
            return 0;

        return _variableInfo.getValue(variableObj.vType().ordinal());
    }
}
