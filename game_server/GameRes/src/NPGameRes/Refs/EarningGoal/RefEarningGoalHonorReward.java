package NPGameRes.Refs.EarningGoal;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "earning_goal_honor_reward")
public class RefEarningGoalHonorReward extends RefBase
{
    private static RefEarningGoalHonorRewardMgr _g_mgr = new RefEarningGoalHonorRewardMgr();

    public static RefEarningGoalHonorRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefEarningGoalHonorRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEarningGoalHonorRewardMgr) _mgr;
    }

    public static class RefEarningGoalHonorRewardMgr extends RefTableContainer<RefEarningGoalHonorReward>
    {

    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEarningGoalHonorReward newRef = (RefEarningGoalHonorReward) _newRef;
        id = newRef.id;
        earning_goal = newRef.earning_goal;
        first_gain_item_list = newRef.first_gain_item_list;
        system_log_id = newRef.system_log_id;
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
    public long system_log_id;//聊天频道系统消息id

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}