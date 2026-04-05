package NPGameRes.Refs.SevenDayGoals;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "seven_day_goals_step_reward")
public class RefSevenDayGoalsStepReward extends RefBase
{
    private static RefSevenDayGoalsStepRewardMgr _g_mgr = new RefSevenDayGoalsStepRewardMgr();

    public static RefSevenDayGoalsStepRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefSevenDayGoalsStepRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefSevenDayGoalsStepRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSevenDayGoalsStepReward newRef = (RefSevenDayGoalsStepReward) _newRef;
        id = newRef.id;
        need_score = newRef.need_score;
        gain_item_list = newRef.gain_item_list;
    }

    public static class RefSevenDayGoalsStepRewardMgr extends RefTableContainer<RefSevenDayGoalsStepReward>
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

    public long id;//id
    public long need_score;//分数
    public List<NPCommonCostItem> gain_item_list;//奖励
}