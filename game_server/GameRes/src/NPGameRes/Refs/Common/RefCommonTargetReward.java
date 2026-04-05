package NPGameRes.Refs.Common;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.List;

@RefTable(tableName = "common_target_reward")
public class RefCommonTargetReward extends RefBase
{
    private static RefTableContainer<RefCommonTargetReward> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefCommonTargetReward> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonTargetReward> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonTargetReward>) _mgr;
    }

    public static class RefCommonTargetRewardMgr extends RefTableContainer<RefCommonTargetReward>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonTargetReward newRef = (RefCommonTargetReward) _newRef;
        id = newRef.id;
        gain_item_list = newRef.gain_item_list;
        process_count = newRef.process_count;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        is_set = newRef.is_set;
        activity_id = newRef.activity_id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public List<NPCommonCostItem> gain_item_list;//资质技能升级消耗(诏书)
    public int process_count;
    public NPPlayerVariableGroupObj process_cur_count; //进度当前值（高级公式）(计数器的话可以不用配置)(进度条计数目标达到时就算完成)
    public NPCountRate trigger_count_rate; //触发器count累计到进度值的倍率
    public String trigger_event = null; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public boolean is_set; //默认false：使用变量，true-使用当前量
    public long activity_id; //关联活动id
}
