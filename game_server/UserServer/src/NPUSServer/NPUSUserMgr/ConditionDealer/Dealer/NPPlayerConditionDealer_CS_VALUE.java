package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_VALUE;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerConditionDealer_CS_VALUE extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_VALUE;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_VALUE cond = (NPPlayerCondition_CS_VALUE) _cond;

        long value = _userData.getValue(cond.conditionValueType());

        return NPPlayerConditionDealerMgr.isRange(value, cond.minValue(), cond.maxValue());
    }
}
