package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_SCOPE_ITEM_COUNT;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_SCOPE_ITEM_COUNT extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_SCOPE_ITEM_COUNT;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_SCOPE_ITEM_COUNT obj = (NPPlayerVariable_CS_SCOPE_ITEM_COUNT) _variableObj;

        long totalCount = 0;
        for (long i = obj.startSubId(); i <= obj.endSubId(); i++)
        {
            totalCount += _userData.getItemCount(obj.itemType(), i);
        }

        return totalCount;
    }
}
