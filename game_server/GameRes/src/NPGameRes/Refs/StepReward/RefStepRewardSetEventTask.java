package NPGameRes.Refs.StepReward;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ERankingEventListenerType;
import NPEnum.ERankingEventServerType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.Refs._ARefRankingEvent;

@RefTable(tableName = "step_reward_set_event_task")
public class RefStepRewardSetEventTask extends _ARefRankingEvent
{
    private static RefStepRewardSetEventTaskMgr _g_mgr = new RefStepRewardSetEventTaskMgr();

    public static RefStepRewardSetEventTaskMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefStepRewardSetEventTaskMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefStepRewardSetEventTaskMgr) _mgr;
    }

    public static class RefStepRewardSetEventTaskMgr extends RefTableContainer<RefStepRewardSetEventTask>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefStepRewardSetEventTask newRef = (RefStepRewardSetEventTask) _newRef;
        id = newRef.id;
        event_trigger_server = newRef.event_trigger_server;
        logic_event = newRef.logic_event;
        trigger_condition = newRef.trigger_condition;
        trigger_count_rate = newRef.trigger_count_rate;
        process_cur_count = newRef.process_cur_count;
        is_set = newRef.is_set;
        set_greater = newRef.set_greater;
        step_reward_set_id = newRef.step_reward_set_id;
        process_limit = newRef.process_limit;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author mark.w
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //排行榜ID
    public ERankingEventServerType event_trigger_server; //事件触发服务器类型
    public String logic_event = null; //监控事件枚举列表
    public NPPlayerConditionGroupObj trigger_condition = new NPPlayerConditionGroupObj(); //触发器-触发条件
    public NPCountRate trigger_count_rate;//触发器count累计到进度值的倍率
    public NPPlayerVariableGroupObj process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
    public boolean is_set;//是否设置值
    public boolean set_greater;//设置更大值（在配置设置值才生效）(是否记录最高值）
    
    //GOB-7026【v0.4.1】新增-学院任务冲榜
    //https://www.teambition.com/task/6923d71e7302d4b260359df4
    public long step_reward_set_id;//阶段奖励设定id
    public long process_limit;//最大值（不填表示无限制）

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    @Override
    public ERankingEventListenerType getListenerType()
    {
        return ERankingEventListenerType.STEP_REWARD_EVENT_TASK;
    }

    @Override
    public ERankingEventServerType getTriggerServerType()
    {
        return event_trigger_server;
    }

    @Override
    public int getLogicEventId()
    {
        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(logic_event.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("StepRewardSet logic_event_id is null, please check, StepRewardId:{} event:{}", id, logic_event, new Exception());
            return 0;
        }
        return eventMeta.getEventId();
    }

    @Override
    public NPCountRate getScoreSource()
    {
        return null;
    }

    @Override
    public NPPlayerVariableGroupObj getProcessCurCount()
    {
        return process_cur_count;
    }

    @Override
    public NPCountRate getTriggerCountRate()
    {
        return trigger_count_rate;
    }

    @Override
    public NPPlayerConditionGroupObj getTriggerCondition()
    {
        return trigger_condition;
    }
}
