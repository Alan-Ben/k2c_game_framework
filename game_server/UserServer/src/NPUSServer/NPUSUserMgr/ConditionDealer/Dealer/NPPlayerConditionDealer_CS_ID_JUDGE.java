package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_ID_JUDGE;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.TreasureHuntCompositeInfo;

public class NPPlayerConditionDealer_CS_ID_JUDGE extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_ID_JUDGE;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_ID_JUDGE cond = (NPPlayerCondition_CS_ID_JUDGE) _cond;

        switch (cond.type())
        {
            case STAGE_GOAL_STEP_IS_DONE:
                return _userData.getStageGoalComponent().isSelectStageDone(cond.id());
            case HAD_GAIN_TREASURE_HUNT_ORE:
                return _userData.getTreasureHuntComponent().getOreMgr().lookupOre(cond.id()) != null;
            case HAD_GAIN_TREASURE_HUNT_TREASURE:
                return _userData.getTreasureHuntComponent().getTreasureMgr().lookupTreasure(cond.id()) != null;
            case HAD_COLLECT_TREASURE_HUNT_COMPOSITE:
            {
                TreasureHuntCompositeInfo compositeInfo = _userData.getTreasureHuntComponent().getCompositeMgr().lookupComposite(cond.id());
                return compositeInfo != null && compositeInfo.hadCollectAllOre();
            }
            case HAD_DONE_SYSTEM_QUEST_TASK:
                return _userData.getSystemQuestComponent().hadDoneSystemQuest(cond.id());
            default:
                return false;
        }
    }
}
