package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_S_HAS_CONSORT;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerConditionDealer_S_HAS_CONSORT extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAS_CONSORT;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_S_HAS_CONSORT cond = (NPPlayerCondition_S_HAS_CONSORT) _cond;

        //计算拥有的大臣数量
        int hasConsortCount = 0;
        //遍历计算数量
        for (Long consortId : cond.getConsortIdList())
        {
            //如果拥有该大臣
            if (_userData.getConsortComponent().lookup(consortId) != null)
                hasConsortCount++;
        }

        return hasConsortCount >= cond.getNeedNum();
    }
}
