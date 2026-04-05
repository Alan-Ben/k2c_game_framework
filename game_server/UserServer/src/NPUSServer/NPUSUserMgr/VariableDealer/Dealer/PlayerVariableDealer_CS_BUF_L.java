package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_BUF_L;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_BUF_L extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_BUF_L;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_BUF_L obj = (NPPlayerVariable_CS_BUF_L) _variableObj;

        PlayerBuffInfo info = _userData.getBuffComponent().lookupBuff(obj.bufId());
        if (null == info)
            return 0L;

        return info.getBo().getLayer();
    }
}
