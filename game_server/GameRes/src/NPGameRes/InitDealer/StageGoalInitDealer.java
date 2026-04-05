package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.StageGoal.RefStageGoal;
import NPGameRes.Refs.StageGoal.RefStageGoalBigStep;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class StageGoalInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        Map<RefStageGoalBigStep, List<RefStageGoal>> bigStepMap = new HashMap<>();

        for (RefStageGoal refStageGoal : RefStageGoal.getMgr().getList())
        {
            RefStageGoalBigStep refBigStep = RefStageGoalBigStep.getMgr().getBigStep(refStageGoal.step);
            if (refBigStep == null)
            {
                CommLog.error("StageGoalInitDealer refBigStep is null, stageGoalId:{}", refStageGoal.Id());
                continue;
            }

            bigStepMap.computeIfAbsent(refBigStep, k -> new ArrayList<>()).add(refStageGoal);
        }

        bigStepMap.forEach((refBigStep, stageGoals) ->
        {

            if (stageGoals.isEmpty())
            {
                CommLog.error("StageGoalInitDealer no stage goals for big step: {}", refBigStep.Id());
                return;
            }

            refBigStep.setStageGoals(stageGoals);
        });
    }
}
