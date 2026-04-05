package NPGameRes.Refs.Rank;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ERankType;
import NPEnum.ERankingEventListenerType;
import NPEnum.ERankingEventServerType;
import NPEnum.ERankingInstanceServerType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.Refs._ARefRankingEvent;

/**
 * @author mark 通用排行榜
 */
@RefTable(tableName = "rank")
public class RefRank extends _ARefRankingEvent
{
    private static RefRefRankCommonMgr _g_mgr = new RefRefRankCommonMgr();

    public static RefRefRankCommonMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRefRankCommonMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRefRankCommonMgr) _mgr;
    }

    public static class RefRefRankCommonMgr extends RefTableContainer<RefRank>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRank newRef = (RefRank) _newRef;
        rank_id = newRef.rank_id;
        name = newRef.name;
        rank_type = newRef.rank_type;
        event_trigger_server = newRef.event_trigger_server;
        instance_hold_server = newRef.instance_hold_server;
        logic_event = newRef.logic_event;
        score_source = newRef.score_source;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_condition = newRef.trigger_condition;
        rank_num_max = newRef.rank_num_max;
        is_set = newRef.is_set;
        set_greater = newRef.set_greater;
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
        return rank_id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long rank_id; //排行榜ID
    public String name; //排行榜名称
    public ERankType rank_type = ERankType.NONE; //主体类型 个人or联盟
    public ERankingEventServerType event_trigger_server; //事件触发服务器类型
    public ERankingInstanceServerType instance_hold_server; //实例创建服务器类型
    public String logic_event = null; //排行榜事件类型
    public NPCountRate score_source;//分数来源(如：大臣id)
    public NPPlayerVariableGroupObj process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
    public NPCountRate trigger_count_rate;//触发器count累计到进度值的倍率
    public NPPlayerConditionGroupObj trigger_condition = new NPPlayerConditionGroupObj(); //触发器-触发条件

    public int rank_num_max; //上榜人数
    public boolean is_set; //默认false：使用变量，true-使用当前量
    public boolean set_greater; //设置更大值（在配置设置值才生效）

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    @Override
    public ERankingEventListenerType getListenerType()
    {
        return ERankingEventListenerType.RANK;
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
            CommLog.error("Rank logic_event_id is null, please check, rankId:{} event:{}", rank_id, logic_event,new Exception());
            return 0;
        }
        return eventMeta.getEventId();
    }

    @Override
    public NPCountRate getScoreSource()
    {
        return score_source;
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
