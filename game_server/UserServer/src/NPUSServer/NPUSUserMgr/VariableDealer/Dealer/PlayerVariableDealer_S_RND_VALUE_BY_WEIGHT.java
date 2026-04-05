package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.PlayerVariable_S_RND_VALUE_BY_WEIGHT;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_S_RND_VALUE_BY_WEIGHT extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.S_RND_VALUE_BY_WEIGHT;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        PlayerVariable_S_RND_VALUE_BY_WEIGHT obj = (PlayerVariable_S_RND_VALUE_BY_WEIGHT) _variableObj;

        return obj.random();
    }
}
