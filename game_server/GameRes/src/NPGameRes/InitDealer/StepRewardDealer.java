package NPGameRes.InitDealer;

import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPGameRes.Refs.StepReward.RefStepRewardSetEventTask;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class StepRewardDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        List<RefStepRewardSet> setList = RefStepRewardSet.getMgr().getList();
        List<RefStepRewardSetEventTask> eventTaskList = RefStepRewardSetEventTask.getMgr().getList();
        
        Map<Long, ArrayList<RefStepRewardSetEventTask>> eventTaskMap = new HashMap<>();
        for (int i = 0; i < eventTaskList.size(); i++)
        {
        	RefStepRewardSetEventTask ref = eventTaskList.get(i);
        	if(null == ref)
        		continue;
        
        	eventTaskMap.computeIfAbsent(ref.step_reward_set_id, k -> new ArrayList<>()).add(ref);
        }
        for(int i = 0; i < setList.size(); i++)
        {
        	RefStepRewardSet ref = setList.get(i);
        	if(null == ref)
        		continue;
        
        	ArrayList<RefStepRewardSetEventTask> refList = eventTaskMap.get(ref.id);
        	if(null != refList)
        	{
        		ref.eventTaskRefList = refList;
        	}
        }
    }
}
