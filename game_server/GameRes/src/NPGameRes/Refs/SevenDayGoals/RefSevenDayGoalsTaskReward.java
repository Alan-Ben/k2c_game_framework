package NPGameRes.Refs.SevenDayGoals;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "seven_day_goals_task_reward")
public class RefSevenDayGoalsTaskReward extends RefBase
{
    private static RefSevenDayGoalsTaskRewardMgr _g_mgr = new RefSevenDayGoalsTaskRewardMgr();

    public static RefSevenDayGoalsTaskRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefSevenDayGoalsTaskRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefSevenDayGoalsTaskRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSevenDayGoalsTaskReward newRef = (RefSevenDayGoalsTaskReward) _newRef;
        id = newRef.id;
        day = newRef.day;
        goal_count = newRef.goal_count;
        reward_item_list = newRef.reward_item_list;
        task_id = newRef.task_id;
        gain_score = newRef.gain_score;
    }

    public static class RefSevenDayGoalsTaskRewardMgr extends RefTableContainer<RefSevenDayGoalsTaskReward>
    {

    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////

    public long id;
    public int day;//天数
    public long goal_count;//目标计数
    public List<NPCommonCostItem> reward_item_list;//奖励
    public long task_id;//关联任务id
    public int gain_score;//完成任务获得的积分
}