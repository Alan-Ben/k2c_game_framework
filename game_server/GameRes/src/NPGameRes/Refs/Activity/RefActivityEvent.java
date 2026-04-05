package NPGameRes.Refs.Activity;

import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

/**
 * 活动触发事件配置表
 */
@RefTable(tableName = "activity_event")
public class RefActivityEvent extends RefBase
{
    private static RefActivityEventMgr _g_mgr = new RefActivityEventMgr();

    public static RefActivityEventMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityEventMgr) _mgr;
    }

    public static class RefActivityEventMgr extends RefTableContainer<RefActivityEvent>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityEvent newRef = (RefActivityEvent) _newRef;
        activity_id = newRef.activity_id;
        logic_event = newRef.logic_event;
        trigger_condition = newRef.trigger_condition;
        effects = newRef.effects;
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
        return activity_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long activity_id;
    public String logic_event = null; //排行榜事件类型
    public NPPlayerConditionGroupObj trigger_condition = new NPPlayerConditionGroupObj(); //触发器-触发条件
    public NPPlayerEffectListParse effects; //触发后效果

    /**
     * 获取触发事件ID
     * @return
     */
    public int getTriggerEventId()
    {
        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(logic_event.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("ActivityEvent logic_event_id is null, please check, activity_id:{} event:{}", activity_id, logic_event, new Exception());
            return 0;
        }
        return eventMeta.getEventId();
    }

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
