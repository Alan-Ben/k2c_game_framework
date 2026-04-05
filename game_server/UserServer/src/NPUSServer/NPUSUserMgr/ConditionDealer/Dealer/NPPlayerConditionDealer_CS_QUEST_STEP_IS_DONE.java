package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_QUEST_STEP_IS_DONE;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

public class NPPlayerConditionDealer_CS_QUEST_STEP_IS_DONE extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_QUEST_STEP_IS_DONE;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_QUEST_STEP_IS_DONE cond = (NPPlayerCondition_CS_QUEST_STEP_IS_DONE) _cond;

        //GOB-6817【优化-0】针对修改主线任务配表数据，在任务完成条件判断增加容错机制
        //https://www.teambition.com/task/691c3479f0932419950f6512
        RefQuest ref = RefQuest.getMgr().get(cond.questId());
        if(null == ref)
        {
        	ref = RefQuest.getMgr().get(RefGeneral.Ref().default_quest_id);
        }
        if(null == ref)
        {
        	USLog.error(_userData.getUSServer(), "player:{} cond-CS_QUEST_STEP_IS_DONE fail, not find quest ref.", _userData.getCid());
        	return false;
        }
        
        return _userData.getQuestComponent().isQuestStepDone(ref.quest_id, cond.stepId());
    }
}
