package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 火星空闲人口数量判断条件处理器
 *
 * 判断玩家火星空闲居民数量是否在指定范围内
 */
public class NPPlayerConditionDealer_CS_MARS_IDLE_PEOPLE_NUM extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_MARS_IDLE_PEOPLE_NUM;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM cond = (NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM) _cond;

        // 获取火星空闲居民数量
        long idleNum = _userData.getMarsPeopleComponent().getNumInfo().getIdleNum();

        // 判断是否在范围内
        return NPPlayerConditionDealerMgr.isRange(idleNum, cond.minValue(), cond.maxValue());
    }
}
