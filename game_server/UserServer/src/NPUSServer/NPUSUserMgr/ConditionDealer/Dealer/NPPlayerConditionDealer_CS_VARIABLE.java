package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_VARIABLE;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;

public class NPPlayerConditionDealer_CS_VARIABLE extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_VARIABLE;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_VARIABLE cond = (NPPlayerCondition_CS_VARIABLE) _cond;
        NPPlayerVariableGroupObj variableGroupObj = cond.variable();

        long var = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, variableGroupObj, _varVariableInfo);

        return NPPlayerConditionDealerMgr.isRange(var, cond.min(), cond.max());
    }
}
