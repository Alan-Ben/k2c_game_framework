package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_PROPERTY;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_PROPERTY extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_PROPERTY;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_PROPERTY obj = (NPPlayerVariable_CS_PROPERTY) _variableObj;

        //因为使用了统一的计算控制器，所以这里不需要计算一遍，我们当前值即可当做就是最终值
        //_userData.getPlayerComponent().getPropertyMgr().calculateChgProperties();
        return _userData.getPlayerComponent().getPropertyMgr().getValue(obj.PropertyType());
    }
}
