package NPGameRes.InitDealer;

import NPGameRes.Refs.SevenDayGoals.RefSevenDayGoalsTask;
import NPGameRes.Refs.SevenDayGoals.RefSevenDayGoalsTaskReward;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SevenDayGoalsInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        //按照任务id分组
        Map<Long, List<RefSevenDayGoalsTaskReward>> taskRewardMap = new HashMap<>();
        for (RefSevenDayGoalsTaskReward refTaskReward : RefSevenDayGoalsTaskReward.getMgr().getList())
        {
            taskRewardMap.computeIfAbsent(refTaskReward.task_id, k -> new ArrayList<>()).add(refTaskReward);
        }

        //遍历任务
        for (RefSevenDayGoalsTask ref : RefSevenDayGoalsTask.getMgr().getList())
        {
            if (null == ref)
                continue;

            List<RefSevenDayGoalsTaskReward> refList = taskRewardMap.get(ref.Id());
            if (refList != null)
            {
                //赋值关联的任务奖励
                ref.task_reward_list = refList;
            }
        }
    }
}
