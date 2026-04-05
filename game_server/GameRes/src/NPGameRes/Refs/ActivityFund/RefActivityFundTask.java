package NPGameRes.Refs.ActivityFund;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 活动基金任务配置表
 */
@RefTable(tableName = "activity_fund_task")
public class RefActivityFundTask extends RefBase
{
    private static RefActivityFundTaskMgr _g_mgr = new RefActivityFundTaskMgr();

    public static RefActivityFundTaskMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityFundTaskMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityFundTaskMgr) _mgr;
    }

    public static class RefActivityFundTaskMgr extends RefTableContainer<RefActivityFundTask>
    {
        // 按任务组ID分组的任务列表
        private Map<Long, List<RefActivityFundTask>> _m_taskMapByGroupId = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            // 按任务组ID分组
            Map<Long, List<RefActivityFundTask>> tempMap = new HashMap<>();
            for (RefActivityFundTask ref : getList())
            {
                if (null == ref)
                    continue;

                tempMap.computeIfAbsent(ref.task_group_id, k -> new ArrayList<>()).add(ref);
            }

            _m_taskMapByGroupId = tempMap;
        }

        /**
         * 获取指定任务组的任务列表
         *
         * @param _groupId 任务组ID
         * @return 任务列表
         */
        public List<RefActivityFundTask> getTaskListByGroupId(long _groupId)
        {
            return _m_taskMapByGroupId.get(_groupId);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityFundTask newRef = (RefActivityFundTask) _newRef;
        task_id = newRef.task_id;
        task_group_id = newRef.task_group_id;
        task_finish_limit = newRef.task_finish_limit;
        done_task_count = newRef.done_task_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        is_set = newRef.is_set;
        gain_score = newRef.gain_score;
    }

    @Override
    public long Id()
    {
        return task_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 任务ID
    public long task_id;

    // 任务组ID
    public long task_group_id;

    // 任务可完成次数上限
    public int task_finish_limit;

    // 任务完成需要的计数
    public long done_task_count;

    // 触发器count累计到进度值的倍率
    public NPCountRate trigger_count_rate;

    // 触发器-事件
    public String trigger_event;

    // 触发器-触发条件
    public NPPlayerConditionGroupObj trigger_condition;

    // 是否设置值（TRUE=覆盖模式，FALSE=增量统计模式）
    public boolean is_set;

    // 任务完成获得分数
    public int gain_score;
}
