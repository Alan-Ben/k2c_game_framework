package NPGameRes.Refs.SevenDayGoals;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "seven_day_goals_task")
public class RefSevenDayGoalsTask extends RefBase
{
    private static RefSevenDayGoalsTaskMgr _g_mgr = new RefSevenDayGoalsTaskMgr();

    public static RefSevenDayGoalsTaskMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefSevenDayGoalsTaskMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefSevenDayGoalsTaskMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSevenDayGoalsTask newRef = (RefSevenDayGoalsTask) _newRef;
        task_id = newRef.task_id;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        is_set = newRef.is_set;
    }

    public static class RefSevenDayGoalsTaskMgr extends RefTableContainer<RefSevenDayGoalsTask>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
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
        return task_id;
    }

    //////////////////////////////

    public long task_id;//任务Id
    public NPPlayerVariableGroupObj process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
    public NPCountRate trigger_count_rate; //触发器count累计到进度值的倍率
    public String trigger_event = null; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public boolean is_set; //默认false：使用变量，true-使用当前量

    @RefField(isIgnore = true)
    public int trigger_event_id;
    @RefField(isIgnore = true)
    public List<RefSevenDayGoalsTaskReward> task_reward_list = new ArrayList<>();

}