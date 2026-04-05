package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_QUEST_DOING;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerConditionDealer_CS_QUEST_DOING extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_QUEST_DOING;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_QUEST_DOING cond = (NPPlayerCondition_CS_QUEST_DOING) _cond;

        return _userData.getQuestComponent().isQuestDoing(cond.questId(), cond.questStep(), cond.questStepTarget());
    }
}
