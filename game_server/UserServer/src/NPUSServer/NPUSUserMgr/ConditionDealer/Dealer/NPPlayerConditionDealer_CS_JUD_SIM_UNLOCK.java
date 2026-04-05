package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_JUD_SIM_UNLOCK;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPGameRes.Refs.RefSimpleUnlock;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerConditionDealer_CS_JUD_SIM_UNLOCK extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_JUD_SIM_UNLOCK;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_JUD_SIM_UNLOCK cond = (NPPlayerCondition_CS_JUD_SIM_UNLOCK) _cond;
        //获取数据
        RefSimpleUnlock unlockRef = RefSimpleUnlock.getMgr().get(cond.simUnlockId());
        if (null == unlockRef)
            return false;

        return NPPlayerConditionDealerMgr.IsEnable(unlockRef.condition_info, _userData, null);
    }
}
