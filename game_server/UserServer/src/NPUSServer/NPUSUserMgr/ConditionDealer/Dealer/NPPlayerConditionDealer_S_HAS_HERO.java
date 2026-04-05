package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_S_HAS_HERO;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerConditionDealer_S_HAS_HERO extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAS_HERO;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_S_HAS_HERO cond = (NPPlayerCondition_S_HAS_HERO) _cond;

        //计算拥有的大臣数量
        int hasHeroCount = 0;
        //遍历计算数量
        for (Long heroId : cond.getHeroIdList())
        {
            //如果拥有该大臣
            if (_userData.getHeroComponent().lookupHero(heroId) != null)
                hasHeroCount++;
        }

        return hasHeroCount >= cond.getNeedNum();
    }
}
