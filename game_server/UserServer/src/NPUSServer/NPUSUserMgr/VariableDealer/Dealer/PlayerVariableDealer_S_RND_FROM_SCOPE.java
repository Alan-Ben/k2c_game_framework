package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_S_RND_FROM_SCOPE;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_S_RND_FROM_SCOPE extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.S_RND_FROM_SCOPE;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_S_RND_FROM_SCOPE obj = (NPPlayerVariable_S_RND_FROM_SCOPE) _variableObj;
        return CommonFunc.randomInt(obj.minValue(), obj.maxValue());
    }
}
