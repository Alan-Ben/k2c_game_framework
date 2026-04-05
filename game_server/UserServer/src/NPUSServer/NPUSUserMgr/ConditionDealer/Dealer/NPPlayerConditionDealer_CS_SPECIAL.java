package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPEnum.ENPPlayer_CS_SpecialCondition;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_SPECIAL;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerConditionDealer_CS_SPECIAL extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_SPECIAL;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_SPECIAL cond = (NPPlayerCondition_CS_SPECIAL) _cond;

        /**
         * 补丁
         * GOB-6753 【BUG-0】火星-阶段目标卡在7-1 https://www.teambition.com/task/691ab5c27191eb29cad104ac
         */
        if(ENPPlayer_CS_SpecialCondition.CS_IS_ARRIVE_MARS == cond.type())
        {
        	return _userData.getMarsGoRouteComponent().isAllDone();
        }
        
        switch (cond.type())
        {
            case CS_QUEST_ALL_DONE:
                return _userData.getQuestComponent().isMainQuestAllDone();
            default:
                return false;
        }
    }
}
