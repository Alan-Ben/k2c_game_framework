package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_BUFF_HAD_ACTIVE_DAY extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_BUFF_HAD_ACTIVE_DAY;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY obj = (NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY) _variableObj;

        PlayerBuffInfo info = _userData.getBuffComponent().lookupBuff(obj.buffId());
        if (null == info)
            return 0L;

        long passMs = Math.max(CommonFunc.getNowTimeMS() - CommonFunc.getZeroClockMS(info.getStartMs()), 0);
        if (passMs == 0)
            return 0L;

        // 向上取整
        return (long) Math.ceil(1.0d * passMs / (CommonFunc.DAY_SEC * 1000L));
    }
}
