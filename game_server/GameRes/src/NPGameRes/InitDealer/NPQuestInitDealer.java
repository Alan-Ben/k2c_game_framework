package NPGameRes.InitDealer;


import Common.QuestEnum.EDailyQuestType;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestActiveReward;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestRefresh;
import NPGameRes.Refs.Quest.MainQuestMgr;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPGameRes.Refs.Quest.RefQuestTarget;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/*******************
 * 初始化 Quest 中子集数据
 * @author Administrator
 *
 */
public class NPQuestInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        //region 日常任务相关
        //1.将日常任务活跃度奖励按照任务类型分组，放到对应的刷新配表中
        Map<EDailyQuestType, List<RefDailyQuestActiveReward>> activeRewardMap = new HashMap<>();
        for (RefDailyQuestActiveReward refDailyQuestActiveReward : RefDailyQuestActiveReward.getMgr().getList())
        {
            activeRewardMap.computeIfAbsent(refDailyQuestActiveReward.daily_quest_type, k -> new ArrayList<>()).add(refDailyQuestActiveReward);
        }
        for (Map.Entry<EDailyQuestType, List<RefDailyQuestActiveReward>> entry : activeRewardMap.entrySet())
        {
            RefDailyQuestRefresh refresh = RefDailyQuestRefresh.getMgr().get(entry.getKey().ordinal());
            if (refresh == null)
                continue;

            refresh.setActiveRewardList(entry.getValue());
        }
        //endregion

        //region 常规任务相关
        List<RefQuest> questRefList = RefQuest.getMgr().getList();
        for (RefQuest ref : questRefList)
        {
            if (ref == null)
                continue;

            List<RefQuestStep> listStep = new ArrayList<>();

            long step_id = ref.first_step_id;
            RefQuestStep refQuestStep;
            //顺序加载阶段任务
            do
            {
                refQuestStep = RefQuestStep.getMgr().get(step_id);
                if (refQuestStep == null)
                {
                    CommLog.error("can not load questStep, ref is null,quest:{}, nextStep:{}", ref.quest_id, step_id);
                    break;
                }

                //防止死循环
                if (listStep.contains(refQuestStep))
                {
                    CommLog.error("load refQuestStep Endless loop,quest:{}, nextStep:{}", ref.quest_id, step_id);
                    break;
                }
                listStep.add(refQuestStep);
                step_id = refQuestStep.next_step_id;
            }
            while (refQuestStep.next_step_id > 0);

            ref.setListStep(listStep);
        }
        //endregion

        //目标任务初始化
        List<RefQuestTarget> targetRefList = RefQuestTarget.getMgr().getList();
        HashMap<Long, ArrayList<RefQuestTarget>> stepTargetMap = new HashMap<>();
        for(int i = 0; i < targetRefList.size(); i++)
        {
            RefQuestTarget targetRef = targetRefList.get(i);
            if(null == targetRef)
                continue;

            stepTargetMap.computeIfAbsent(targetRef.step_id, k -> new ArrayList<>()).add(targetRef);
        }
        stepTargetMap.forEach((step, stepTargetRefList) ->
        {
            RefQuestStep stepRef = RefQuestStep.getMgr().get(step);
            if(null == stepRef)
            {
                CommLog.error("can not find stepRef for stepId:{}", step);
                return;
            }

            stepRef.setListStep(stepTargetRefList);
        });

        //同步主线任务链条
        MainQuestMgr.getInstance().init();
    }

}
