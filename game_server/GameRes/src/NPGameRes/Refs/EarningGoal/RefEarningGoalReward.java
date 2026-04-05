package NPGameRes.Refs.EarningGoal;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "earning_goal_reward")
public class RefEarningGoalReward extends RefBase
{
    private static RefEarningGoalRewardMgr _g_mgr = new RefEarningGoalRewardMgr();

    public static RefEarningGoalRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefEarningGoalRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEarningGoalRewardMgr) _mgr;
    }

    public static class RefEarningGoalRewardMgr extends RefTableContainer<RefEarningGoalReward>
    {

    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEarningGoalReward newRef = (RefEarningGoalReward) _newRef;
        id = newRef.id;
        earning_goal = newRef.earning_goal;
        first_gain_item_list = newRef.first_gain_item_list;
        all_gain_item_list = newRef.all_gain_item_list;
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

    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//唯一id
    public long earning_goal;//赚速目标
    public List<NPCommonCostItem> first_gain_item_list;//首达奖励
    public List<NPCommonCostItem> all_gain_item_list;//全民奖励

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}