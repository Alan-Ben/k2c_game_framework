package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_CONSORT_LIKE_COUNT;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelConsortInfo;

/**
 * 妃子好感度，妃子不能是已获取
 * @author mj
 *
 */
public class NPPlayerConditionDealer_CS_CONSORT_LIKE_COUNT extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_CONSORT_LIKE_COUNT;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_CONSORT_LIKE_COUNT cond = (NPPlayerCondition_CS_CONSORT_LIKE_COUNT) _cond;
        
        if(_userData.getConsortComponent().hasConsort(cond.getConsortId()))
        {
        	return false;
        }

        long count = 0;
        TravelConsortInfo consort = _userData.getTravelComponent().lookupConsort(cond.getConsortId());
        if(null != consort)
        {
        	count = consort.getLike();
        }
        
        return NPPlayerConditionDealerMgr.isRange(count, cond.minValue(), cond.maxValue());
    }
}
