package NPGameRes.Refs.Reward;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "reward")
public class RefReward extends RefBase
{
    private static RefRewardMgr _g_mgr = new RefRewardMgr();

    public static RefRewardMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefRewardMgr extends RefTableContainer<RefReward>
    {
    }

    @Override
    public RefRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefReward newRef = (RefReward) _newRef;
        reward_id = newRef.reward_id;
        certainly_drop_item_count_list = newRef.certainly_drop_item_count_list;
        is_merge = newRef.is_merge;
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
        return reward_id;
    }

    ////////////////////////


    public long reward_id;
    public List<NPCommonCostItem> certainly_drop_item_count_list = new ArrayList<>();//certainly_drop_item_count_list
    public boolean is_merge = false;//奖励是否合并（false不合并,true合并,默认false）
}
